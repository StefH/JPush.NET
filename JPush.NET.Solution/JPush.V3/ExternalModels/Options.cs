using System;
using System.Runtime.Serialization;

namespace JPush.V3.ExternalModels
{
    [DataContract]
    public class Options
    {
        /// <summary>
        /// The maximum allowed life time (10 days)
        /// </summary>
        public static int MaxLifeTime = 10 * TimeSpan.FromDays(10).Seconds;

        [DataMember(Name = "sendno", EmitDefaultValue = false)]
        public int? SendNo { get; set; }

        [DataMember(Name = "time_to_live", EmitDefaultValue = false)]
        public int? Ttl { get; set; }

        [DataMember(Name = "override_msg_id", EmitDefaultValue = false)]
        public long? OverrideMsgId { get; set; }

        [DataMember(Name = "apns_production", EmitDefaultValue = false)]
        public bool ApnsProduction { get; set; }
    }
}