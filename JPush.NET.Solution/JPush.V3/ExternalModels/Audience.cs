using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract(Name = "audience")]
    public class Audience
    {
        #region Constants
        /// <summary>
        /// The maximum number of tags
        /// </summary>
        public static int MaxTag = 20;

        /// <summary>
        /// The maximum number of tag_and
        /// </summary>
        public static int MaxTagAnd = 20;

        /// <summary>
        /// The maximum number of aliases
        /// </summary>
        public static int MaxAlias = 1000;

        /// <summary>
        /// The maximum number of registration ids
        /// </summary>
        public static int MaxRegistrationId = 1000;
        #endregion

        [DataMember(Name = "tag", EmitDefaultValue = false)]
        public List<string> Tag { get; set; }

        [DataMember(Name = "tag_and", EmitDefaultValue = false)]
        public List<string> TagAnd { get; set; }

        [DataMember(Name = "alias", EmitDefaultValue = false)]
        public List<string> Alias { get; set; }

        [DataMember(Name = "registration_id", EmitDefaultValue = false)]
        public List<string> RegistrationId { get; set; }
    }
}