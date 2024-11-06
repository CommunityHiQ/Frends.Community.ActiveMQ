using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Community.ActiveMQ
{
    /// <summary>
    /// Input-class for the Consume-Task.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Connection string to ActiveMQ.
        /// </summary>
        /// <example>tcp://localhost:61616</example>
        [PasswordPropertyText]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Queue from which messages will be consumed.
        /// </summary>
        /// <example>myQueue</example>
        [DisplayFormat(DataFormatString = "Text")]
        public string Queue { get; set; }
    }

    /// <summary>
    /// Options-class.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Specifies the maximum duration, in seconds, to wait for receiving a message from the queue.
        /// </summary>
        /// <example>10</example>
        [DefaultValue(10)]
        public int MessageReceiveTimeout { get; set; }

        /// <summary>
        /// Maximum amount of time, in milliseconds, to wait for the message consumption task to complete.
        /// The default value is 5000 milliseconds (5 seconds).
        /// </summary>
        /// <example>5000</example>
        [DefaultValue(5000)]
        public int TaskExecutionTimeout { get; set; }

        /// <summary>
        /// Maximum number of messages to receive. A value of 0 means no limit.
        /// </summary>
        /// <example>5</example>
        [DefaultValue(0)]
        public int MaxMessagesToConsume { get; set; }

        /// <summary>
        /// Should the task throw an error if no messages are consumed?
        /// </summary>
        /// <example>true</example>
        [DefaultValue(false)]
        public bool ThrowErrorIfEmpty { get; set; }
    }

    /// <summary>
    /// Result-class for the Consume-Task.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Messages consumed from the queue.
        /// </summary>
        public Message[] Messages { get; set; }
    }

    /// <summary>
    /// Consume-task's resulting message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Constructor for creating instances as intended.
        /// </summary>
        /// <param name="type">Type of the message.</param>
        /// <param name="content">Content of the message.</param>
        public Message(string type, dynamic content)
        {
            Type = type;
            Content = content;
        }

        /// <summary>
        /// Type of message. Can be "Text", "Bytes".
        /// Other possibilities are "Map", "Object", "Stream", but those are not supported yet.
        /// </summary>
        /// <example>Text</example>
        public string Type { get; }

        /// <summary>
        /// Content of the message with type depending on message type.
        /// For text messages, the content's type is string.
        /// For bytes messages, the content's type is byte[].
        /// Other message types are not supported.
        /// </summary>
        /// <example>Hello, World!</example>
        public dynamic Content { get; }
    }

    /// <summary>
    /// Input-class for the Produce-Task.
    /// </summary>
    public class ProduceInput
    {
        /// <summary>
        /// Connection string to ActiveMQ.
        /// </summary>
        /// <example>tcp://localhost:61616</example>
        [PasswordPropertyText]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Queue to which the message will be sent.
        /// </summary>
        /// <example>myQueue</example>
        [DisplayFormat(DataFormatString = "Text")]
        public string Queue { get; set; }

        /// <summary>
        /// Message which will be sent to the queue.
        /// </summary>
        /// <example>This is a test message.</example>
        [DisplayFormat(DataFormatString = "Text")]
        public string Message { get; set; }
    }

    /// <summary>
    /// Result-class for the Produce-Task.
    /// </summary>
    public class ProduceResult
    {
        /// <summary>
        /// Indicates whether the message was sent successfully.
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }
    }
}