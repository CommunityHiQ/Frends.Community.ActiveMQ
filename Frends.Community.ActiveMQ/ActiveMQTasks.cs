using Apache.NMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Frends.Community.ActiveMQ
{
    /// <summary>
    /// Class for ActiveMQ tasks.
    /// </summary>
    public class ActiveMQTasks
    {
        /// <summary>
        /// Consume messages from ActiveMQ queue. See https://github.com/CommunityHiQ/Frends.Community.ActiveMQ
        /// </summary>
        /// <param name="input">Information about the queue from which messages are consumed.</param>
        /// <param name="options">Options for consuming messages.</param>
        /// <param name="cancellationToken">Automatically passed by Frends. Interrupts Task execution if Process is terminated.</param>
        /// <returns>Object{ string[] Messages }</returns>
        public static async Task<Result> Consume([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
        {
            var messages = new List<Message>();
            var factory = new NMSConnectionFactory(input.ConnectionString);
            using (var connection = await factory.CreateConnectionAsync())
            {
                connection.Start();
                using (var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge))
                using (var queue = await session.GetQueueAsync(input.Queue))
                using (var consumer = await session.CreateConsumerAsync(queue))
                {
                    bool readNextMessage;
                    do
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        readNextMessage = false;

                        var task = Task.Run(() => consumer.Receive(TimeSpan.FromSeconds(options.MessageReceiveTimeout)), cancellationToken);
                        if (task.Wait(options.TaskExecutionTimeout == 0 ? 5000 : options.TaskExecutionTimeout, cancellationToken))
                        {
                            if (task.Result is ITextMessage textMessage)
                            {
                                messages.Add(new Message("Text", textMessage.Text));
                                readNextMessage = true;
                            }
                            else if (task.Result is IStreamMessage streamMessage)
                            {
                                messages.Add(new Message("Stream", "Stream message are not supported"));
                                readNextMessage = true;
                            }
                            else if (task.Result is IBytesMessage bytesMessage)
                            {
                                messages.Add(new Message("Bytes", bytesMessage.Content));
                                readNextMessage = true;
                            }
                            else if (task.Result is IMapMessage mapMessage)
                            {
                                messages.Add(new Message("Map", "Map messages are not supported"));
                                readNextMessage = true;
                            }
                            else if (task.Result is IObjectMessage objectMessage)
                            {
                                messages.Add(new Message("Object", "Object messages are not supported"));
                                readNextMessage = true;
                            }
                            else if (task.Result is null)
                                break;
                            else
                                messages.Add(new Message("Unknown message type", "Unknown message type: " + task.Result.GetType().Name));

                            if (options.MaxMessagesToConsume > 0 &&
                                messages.Count >= options.MaxMessagesToConsume)
                                break;
                        }
                    } while (readNextMessage);
                }
            }
            if (options.ThrowErrorIfEmpty && messages.Count == 0)
                throw new Exception($"No messages consumed from queue {input.Queue}.");
            return new Result { Messages = messages.ToArray() };
        }

        /// <summary>
        /// Send message to ActiveMQ queue. See https://github.com/CommunityHiQ/Frends.Community.ActiveMQ
        /// </summary>
        /// <param name="input">Information about the queue to which message is sent.</param>
        /// <returns>Object{ bool Success }</returns>
        public static async Task<ProduceResult> Produce([PropertyTab] ProduceInput input)
        {
            var factory = new NMSConnectionFactory(input.ConnectionString);
            using (var connection = await factory.CreateConnectionAsync())
            {
                await connection.StartAsync();
                using (var session = await connection.CreateSessionAsync(AcknowledgementMode.AutoAcknowledge))
                using (var dest = await session.GetQueueAsync(input.Queue))
                using (var producer = await session.CreateProducerAsync(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    await producer.SendAsync(session.CreateTextMessage(input.Message));
                }
            }
            return new ProduceResult { Success = true };
        }
    }
}