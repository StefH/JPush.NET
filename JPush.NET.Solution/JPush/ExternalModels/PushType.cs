using System.Runtime.Serialization;

namespace JPush.V2.ExternalModels
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
        /// The broadcast (only supported in V2)
        /// </summary>
        [EnumMember]
        Broadcast = 4,

        /// <summary>
        /// All (=broadcast) (only supported in V3)
        /// </summary>
        [EnumMember]
        All = 4,

        /// <summary>
        /// The by registration unique identifier
        /// </summary>
        [EnumMember]
        ByRegistrationId = 5,

        /// <summary>
        /// The by tag_and (only supported in V3)
        /// </summary>
        [EnumMember]
        ByTagAnd = 6
    }
}