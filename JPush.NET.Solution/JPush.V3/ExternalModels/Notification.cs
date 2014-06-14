using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract]
    public class Notification
    {
        [DataMember(Name = "alert", EmitDefaultValue = false)]
        public string Alert { get; set; }

        [DataMember(Name = "android", EmitDefaultValue = false)]
        public Android Android { get; set; }

        [DataMember(Name = "ios", EmitDefaultValue = false)]
        public Apple Apple { get; set; }

        [DataMember(Name = "winphone", EmitDefaultValue = false)]
        public WindowsPhone WindowsPhone { get; set; }
    }
}