using System.Runtime.Serialization;

namespace JPush.Models
{
    /// <summary>
    /// Enum PushType
    /// </summary>
    [DataContract]
    public enum PushType
    {
        /// <summary>
        /// The unknown
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// The by tag
        /// </summary>
        [EnumMember]
        ByTag = 2,

        /// <summary>
        /// The by alias
        /// </summary>
        [EnumMember]
        ByAlias = 3,

        /// <summary>
        /// The broadcast
        /// </summary>
        [EnumMember]
        Broadcast = 4,

        /// <summary>
        /// The by registration unique identifier
        /// </summary>
        [EnumMember]
        ByRegistrationId = 5
    }
}