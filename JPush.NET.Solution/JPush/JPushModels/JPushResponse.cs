using System.Runtime.Serialization;

namespace JPush.JPushModels
{
    /// <summary>
    /// Class JPushResponse
    /// </summary>
    [DataContract]
    public class JPushResponse
    {
        [DataMember(Name = "sendno")]
        public string SendNo { get; set; }

        [DataMember(Name = "errcode")]
        public int ErrorCode { get; set; }

        [DataMember(Name = "errmsg")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "msg_id")]
        public string MessageId { get; set; }
    }
}