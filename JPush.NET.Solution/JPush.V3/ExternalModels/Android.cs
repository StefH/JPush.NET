using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract(Name = "android")]
    public class Android
    {
        [DataMember(Name = "alert", EmitDefaultValue = false)]
        public string Alert { get; set; }

        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; set; }

        [DataMember(Name = "builder_id", EmitDefaultValue = false)]
        public int? BuilderId { get; set; }

        [DataMember(Name = "extras", EmitDefaultValue = false)]
        public Dictionary<string, string> Extras { get; set; }
    }
}