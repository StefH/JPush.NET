using System.Runtime.Serialization;

namespace JPush.Models
{
    /// <summary>
    /// Enum MessageType
    /// </summary>
    [DataContract]
    public enum MessageType
    {
        [EnumMember]
        None = 0,

        /// <summary>
        /// The notification
        /// </summary>
        [EnumMember]
        Notification = 1,

        /// <summary>
        /// The customized message.
        /// For Android only.
        /// </summary>
        [EnumMember]
        CustomizedMessage = 2
    }
}
