using System.Runtime.Serialization;

namespace JPush.V2.ExternalModels
{
    /// <summary>
    /// Enum PushPlatform
    /// </summary>
    [DataContract]
    public enum PushPlatform
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// Android
        /// </summary>
        [EnumMember]
        Android = 1,

        /// <summary>
        /// iOS
        /// </summary>
        [EnumMember]
        IOS = 2,

        /// <summary>
        /// WindowsPhone
        /// </summary>
        [EnumMember]
        WindowsPhone = 4,

        /// <summary>
        /// Android and iOS
        /// </summary>
        [EnumMember]
        AndroidAndIOS = Android | IOS
    }
}