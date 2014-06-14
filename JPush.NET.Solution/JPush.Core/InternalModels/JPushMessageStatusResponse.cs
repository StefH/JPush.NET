using System.Runtime.Serialization;

namespace JPush.Core.InternalModels
{
    /// <summary>
    /// Class JPushMessageStatus.
    /// </summary>
    [DataContract]
    public class JPushMessageStatusResponse
    {
        /// <summary>
        /// Gets or sets the message unique identifier.
        /// This value is decided by JPush Service.
        /// </summary>
        /// <value>The message unique identifier.</value>
        [DataMember(Name = "msg_id")]
        public string MessageId { get; set; }


        /// <summary>
        /// Gets or sets the android received.
        /// </summary>
        /// <value>
        /// The android received.
        /// </value>
        [DataMember(Name = "android_received")]
        public int? AndroidReceived { get; set; }


        /// <summary>
        /// Gets or sets the apple push notification sent.
        /// </summary>
        /// <value>
        /// The apple push notification sent.
        /// </value>
        [DataMember(Name = "ios_apns_sent")]
        public int? ApplePushNotificationSent { get; set; }
    }
}