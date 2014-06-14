using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract(Name = "winphone")]
    public class WindowsPhone
    {
        [DataMember(Name = "alert", EmitDefaultValue = false)]
        public string Alert { get; set; }

        [DataMember(Name = "title", EmitDefaultValue = false)]
        public string Title { get; set; }

        [DataMember(Name = "_open_page", EmitDefaultValue = false)]
        public bool? OpenPage { get; set; }

        [DataMember(Name = "extras", EmitDefaultValue = false)]
        public Dictionary<string, string> Extras { get; set; }
    }
}
