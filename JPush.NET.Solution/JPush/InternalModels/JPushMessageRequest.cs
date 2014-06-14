using System;
using System.Runtime.Serialization;

namespace JPush.V2.InternalModels
{
    [DataContract]
    [Serializable]
    public class JPushMessageRequest
    {
        // Mandatory parameters
        [DataMember(Name = "sendno")]
        public int SendNo { get; set; }

        [DataMember(Name = "app_key")]
        public string AppKey { get; set; }

        [DataMember(Name = "verification_code")]
        public string VerificationCode { get; set; }

        [DataMember(Name = "receiver_type")]
        public int ReceiverType { get; set; }

        [DataMember(Name = "msg_type")]
        public int MessageType { get; set; }

        [DataMember(Name = "msg_content")]
        public string MessageContent { get; set; }

        [DataMember(Name = "platform")]
        public string Platform { get; set; }

        // Optional parameters
        [DataMember(Name = "receiver_value")]
        public string ReceiverValue { get; set; }

        [DataMember(Name = "send_description")]
        public string Description { get; set; }

        [DataMember(Name = "time_to_live")]
        public int? LifeTime { get; set; }

        [DataMember(Name = "override_msg_id")]
        public string OverrideMessageId { get; set; }
    }

    /*
    if (Platform.Contains(PushPlatform.iOS))
    {
        result.Add("apns_production", IsTestEnvironment ? 0 : 1);
    }
    }*/
}