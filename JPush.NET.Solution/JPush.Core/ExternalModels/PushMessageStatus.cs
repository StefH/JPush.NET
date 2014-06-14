using System.Runtime.Serialization;

namespace JPush.Core.ExternalModels
{
    /// <summary>
    /// Class PushMessageStatus.
    /// </summary>
    [DataContract]
    public class PushMessageStatus
    {
        /// <summary>
        /// Gets or sets the message unique identifier.
        /// This value is decided by JPush Service.
        /// </summary>
        /// <value>The message unique identifier.</value>
        [DataMember(Name = "msg_id")]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the android delivered count.
        /// </summary>
        /// <value>The android delivered count.</value>
        [DataMember(Name = "android_received")]
        public int? AndroidDeliveredCount { get; set; }

        /// <summary>
        /// Gets or sets the apple push notification delivered count.
        /// </summary>
        /// <value>The apple push notification delivered count.</value>
        [DataMember(Name = "ios_apns_sent")]
        public int? ApplePushNotificationDeliveredCount { get; set; }
    }
}
