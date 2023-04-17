﻿using Apache.NMS;
using NUnit.Framework;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frends.Community.ActiveMQ.Tests
{
    [TestFixture]
    public class ConsumeTests
    {
        /*
         * Tests require ActiveMQ to be in use. Please run the following command before executing tests.
         * cd Frends.Community.ActiveMQ.Tests && docker-compose up -d
         * 
         * UI Admin login at http://localhost:8161/admin. User: admin, Pass: admin
         */

        // Another way to put it: tcp://172.28.100.124:62626?nms.username=ABCDE&nms.password=QWERTY
        private readonly string _connectionString = "activemq:tcp://admin:admin@localhost:61616";

        [Test]
        public async Task ConsumeBytesMessagesFromQueue()
        {
            var str = "test message";
            var bytes = Encoding.UTF8.GetBytes(str);
            await SendBytesMessageToQueue(bytes);
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "testqueue"
            };

            var options = new Options
            {
                ThrowErrorIfEmpty = true
            };
            var result = await ActiveMQTasks.Consume(input, options, new CancellationToken());
            Assert.IsTrue(result.Messages.Length > 0);
            var bytesReceived = (byte[])result.Messages[0].Content;
            var strReceived = Encoding.UTF8.GetString(bytesReceived);
            Assert.AreEqual(strReceived,  result.Messages[0].Content);
        }

        [Test]
        public async Task ConsumeMessagesFromQueue()
        {
            await SendMessageToQueue("test message");
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "testqueue"
            };

            var options = new Options
            {
                ThrowErrorIfEmpty = true
            };
            var result = await ActiveMQTasks.Consume(input, options, new CancellationToken());
            Assert.IsTrue(result.Messages.Length > 0);
            Assert.AreEqual("test message", result.Messages[0].Content);
        }

        [Test]
        public void EmptyQueueDoesNotThrowException()
        {
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "emptyqueue"
            };

            var options = new Options
            {
                ThrowErrorIfEmpty = false
            };
            Assert.DoesNotThrowAsync(async () => await ActiveMQTasks.Consume(input, options, new CancellationToken()));
        }

        [Test]
        public void EmptyQueueThrowsException()
        {
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "emptyqueue"
            };

            var options = new Options
            {
                ThrowErrorIfEmpty = true
            };

            var error = Assert.ThrowsAsync<Exception>(async () => await ActiveMQTasks.Consume(input, options, new CancellationToken()));
            Assert.AreEqual("No messages consumed from queue emptyqueue.", error.Message);
        }

        #region HelperMethods

        private async Task SendBytesMessageToQueue(byte[] message)
        {
            var factory = new NMSConnectionFactory(_connectionString);

            using (var connection = await factory.CreateConnectionAsync())
            {
                await connection.StartAsync();

                using (var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge))
                using (var dest = await session.GetQueueAsync("testqueue"))
                using (var producer = await session.CreateProducerAsync(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    var bytesMessage = session.CreateBytesMessage();
                    bytesMessage.WriteBytes(message);
                    producer.Send(bytesMessage);
                }
            }
        }

        private async Task SendMessageToQueue(string message)
        {
            var factory = new NMSConnectionFactory(_connectionString);

            using (var connection = await factory.CreateConnectionAsync())
            {
                await connection.StartAsync();

                using (var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge))
                using (var dest = await session.GetQueueAsync("testqueue"))
                using (var producer = await session.CreateProducerAsync(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    producer.Send(session.CreateTextMessage(message));
                }
            }
        }

        #endregion
    }
}
