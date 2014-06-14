using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract(Name = "ios")]
    public class Apple
    {
        [DataMember(Name = "alert", EmitDefaultValue = false)]
        public string Alert { get; set; }

        [DataMember(Name = "Sound", EmitDefaultValue = false)]
        public string Sound { get; set; }

        [DataMember(Name = "btch", EmitDefaultValue = false)]
        public int? Batch { get; set; }

        [DataMember(Name = "content-available", EmitDefaultValue = false)]
        public bool? ContentAvailableBatch { get; set; }

        [DataMember(Name = "extras", EmitDefaultValue = false)]
        public Dictionary<string, string> Extras { get; set; }
    }
}