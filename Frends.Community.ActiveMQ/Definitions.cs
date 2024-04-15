using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Community.ActiveMQ
{
    /// <summary>
    /// Input-class for Consume-Task.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Connection string to ActveMQ.
        /// </summary>
        [PasswordPropertyText]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Queue from which messages will be consumed.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Queue { get; set; }
    }

    /// <summary>
    /// Options-class for Consume-Task.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Timeout for receiving messages. Timeout affects to single message.
        /// </summary>
        [DefaultValue(10)]
        public int Timeout { get; set; }

        /// <summary>
        /// Maximum number of messages to receive. 0 means no limit.
        /// </summary>
        [DefaultValue(0)]
        public int MaxMessagesToConsume { get; set; }
        
        
        /// <summary>
        /// Should the Task throw error if no messages are consumed?
        /// </summary>
        [DefaultValue(false)]
        [DisplayName("Throw error if no messages are consumed")]
        public bool ThrowErrorIfEmpty { get; set; }
    }

    /// <summary>
    /// Result-class for Consume-Task.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Messages consumed from queue.
        /// </summary>
        public Message[] Messages { get; set; }
    }

    /// <summary>
    /// Consume task's resulting message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Constructor for creating instances as intended.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        public Message(string type, dynamic content)
        {
            Type = type;
            Content = content;
        }

        /// <summary>
        /// Type of message. Can be "Text", "Bytes".
        /// Other possibilities are "Map", "Object", "Stream", but those are
        /// not supported yet.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Content of the message with type depending on message type.
        /// For Text messages the Content's type is string,
        /// for Bytes messages the Content's type is byte[].
        /// Other message types are not supported.
        /// </summary>
        public dynamic Content { get; }
    }

    /// <summary>
    /// Input-class for Produce-Task.
    /// </summary>
    public class ProduceInput
    {
        /// <summary>
        /// Connection string to ActveMQ.
        /// </summary>
        [PasswordPropertyText]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Queue to which message will be sent.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Queue { get; set; }

        /// <summary>
        /// Message which will be sent to the queue.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string Message { get; set; }
    }

    /// <summary>
    /// Result-class for Produce-Task.
    /// </summary>
    public class ProduceResult
    {
        /// <summary>
        /// Message was sent successfully?
        /// </summary>
        public bool Success { get; set; }
    }
}
