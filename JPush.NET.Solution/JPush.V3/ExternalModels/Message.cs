using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract(Name = "message")]
    public class Message
    {
        [DataMember(Name = "msg_content", EmitDefaultValue = false)]
        public string Content { get; set; }

        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; set; }

        [DataMember(Name = "content_type", EmitDefaultValue = false)]
        public string ContentType { get; set; }

        [DataMember(Name = "extras", EmitDefaultValue = false)]
        public Dictionary<string, string> Extras { get; set; }
    }
}