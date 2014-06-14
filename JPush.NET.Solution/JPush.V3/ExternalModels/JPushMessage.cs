using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract]
    [KnownType(typeof(Audience))]
    [KnownType(typeof(List<string>))]
    public class JPushMessage
    {
        [DataMember(Name = "platform", EmitDefaultValue = false)]
        public object Platform { get; set; }

        [DataMember(Name = "audience", EmitDefaultValue = false)]
        public object Audience { get; set; }

        [DataMember(Name = "notification", EmitDefaultValue = false)]
        public Notification Notification { get; set; }

        [DataMember(Name = "message", EmitDefaultValue = false)]
        public Message Message { get; set; }

        [DataMember(Name = "options", EmitDefaultValue = false)]
        public Options Options { get; set; }
    }
}
