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
        public string[] Messages { get; set; }
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
