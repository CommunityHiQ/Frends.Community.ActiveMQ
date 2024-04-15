using Apache.NMS;
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

        [TearDown]
        public async Task TearDown()
        {
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "testqueue"
            };

            var options = new Options
            {
                Timeout = 1000,
                MaxMessagesToConsume = 0,
                ThrowErrorIfEmpty = false
            };
            await ActiveMQTasks.Consume(input, options, default);
        }

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
                Timeout = 5,
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
            await SendMessageToQueue("test message", 1);
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "testqueue"
            };

            var options = new Options
            {
                Timeout = 1,
                MaxMessagesToConsume = 0,
                ThrowErrorIfEmpty = false
            };
            var result = await ActiveMQTasks.Consume(input, options, new CancellationToken());
            Assert.IsTrue(result.Messages.Length > 0);
            Assert.AreEqual("test message", result.Messages[0].Content);
            Assert.AreEqual(1, result.Messages.Length);
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
                Timeout = 5,
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
                Timeout = 5,
                ThrowErrorIfEmpty = true
            };

            var error = Assert.ThrowsAsync<Exception>(async () => await ActiveMQTasks.Consume(input, options, new CancellationToken()));
            Assert.AreEqual("No messages consumed from queue emptyqueue.", error.Message);
        }
        
        [Test]
        public async Task MaxMessagesToConsumeTest()
        {
            await SendMessageToQueue("test message 1");
            await SendMessageToQueue("test message 2");
            await SendMessageToQueue("test message 3");
            
            var input = new Input
            {
                ConnectionString = _connectionString,
                Queue = "testqueue"
            };
            var options = new Options
            {
                Timeout = 5,
                MaxMessagesToConsume = 2
            };
            
            var result = await ActiveMQTasks.Consume(input, options, new CancellationToken());
            Assert.AreEqual(2, result.Messages.Length);
            Assert.AreEqual("test message 1", result.Messages[0].Content);
            Assert.AreEqual("test message 2", result.Messages[1].Content);
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

        private async Task SendMessageToQueue(string message, int count = 1)
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
                    for (int i = 0; i < count; i++)
                    {
                        producer.Send(session.CreateTextMessage(message));
                    }
                }
            }
        }

        #endregion
    }
}
