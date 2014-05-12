using System.Runtime.Serialization;

namespace JPush.Models
{
    /// <summary>
    /// Enum PushResponseCode
    /// </summary>
    [DataContract]
    public enum PushResponseCode
    {
        /// <summary>
        /// The succeed
        /// </summary>
        [EnumMember]
        Succeed = 0,

        /// <summary>
        /// The service error
        /// </summary>
        [EnumMember]
        ServiceError = 10,

        /// <summary>
        /// The post only
        /// </summary>
        [EnumMember]
        PostOnly = 1001,

        /// <summary>
        /// The missing required parameter
        /// </summary>
        [EnumMember]
        MissingRequiredParameter = 1002,

        /// <summary>
        /// The invalid parameter
        /// </summary>
        [EnumMember]
        InvalidParameter = 1003,

        /// <summary>
        /// The failed verification code
        /// </summary>
        [EnumMember]
        FailedVerificationCode = 1004,

        /// <summary>
        /// The body too large
        /// </summary>
        [EnumMember]
        BodyTooLarge = 1005,

        /// <summary>
        /// The invalid user or password
        /// </summary>
        [EnumMember]
        InvalidUserOrPassword = 1006,

        /// <summary>
        /// The invalid receiver value
        /// </summary>
        [EnumMember]
        InvalidReceiverValue = 1007,

        /// <summary>
        /// The invalid application key
        /// </summary>
        [EnumMember]
        InvalidAppKey = 1008,

        /// <summary>
        /// The invalid message content
        /// </summary>
        [EnumMember]
        InvalidMessageContent = 1010,

        /// <summary>
        /// The no destination reached
        /// </summary>
        [EnumMember]
        NoDestinationReached = 1011,

        /// <summary>
        /// The customized message not support
        /// </summary>
        [EnumMember]
        CustomizedMessageNotSupport = 1012,

        /// <summary>
        /// The invalid content type
        /// </summary>
        [EnumMember]
        InvalidContentType = 1013
    }
}
