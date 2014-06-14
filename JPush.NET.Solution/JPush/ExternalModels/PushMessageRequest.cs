using System;
using System.Runtime.Serialization;

namespace JPush.V2.ExternalModels
{
    /// <summary>
    /// Class PushMessageRequest.
    /// </summary>
    [DataContract]
    [KnownType(typeof(PushMessage))]
    [KnownType(typeof(PushType))]
    [KnownType(typeof(MessageType))]
    [KnownType(typeof(PushPlatform))]
    public class PushMessageRequest
    {
        /// <summary>
        /// The maximum allowed life time (10 days)
        /// </summary>
        public static int MaxLifeTime = 10 * TimeSpan.FromDays(10).Seconds;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public PushMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember]
        public PushType PushType { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        [DataMember]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        [DataMember]
        public PushPlatform Platform { get; set; }

        /// <summary>
        /// Gets or sets the receiver value.
        /// </summary>
        /// <value>The receiver value.</value>
        [DataMember]
        public string ReceiverValue { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the life time.
        /// Unit: second.
        /// Max: 864000 Seconds (10 days).
        /// Default: 86400 Seconds (1 days).      
        /// </summary>
        /// <value>The life time.</value>
        [DataMember]
        public int LifeTime { get; set; }

        /// <summary>
        /// Gets or sets the override message unique identifier.
        /// </summary>
        /// <value>The override message unique identifier.</value>
        [DataMember]
        public string OverrideMessageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is test.
        /// For iOS only.
        /// Default: false.
        /// </summary>
        /// <value><c>true</c> if this instance is test; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool IsTestEnvironment { get; set; }
    }
}
