using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JPush.Extensions;

namespace JPush.Models
{
    /// <summary>
    /// Class PushMessageRequest.
    /// <see cref="PushMessageRequest"/> and <see cref="PushMessage"/> work together to send out push request.
    /// See official RESTful API: http://docs.jpush.cn/display/dev/Push+API+v2
    /// </summary>
    [DataContract]
    [KnownType(typeof(PushMessage))]
    [KnownType(typeof(PushType))]
    [KnownType(typeof(MessageType))]
    [KnownType(typeof(PushPlatform))]
    public class PushMessageRequest
    {
        /// <summary>
        /// The maximum allowed life time (10 days)
        /// </summary>
        private static int MaxLifeTime = 10 * TimeSpan.FromDays(10).Seconds;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public PushMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember]
        public PushType PushType { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        [DataMember]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        [DataMember]
        public PushPlatform Platform { get; set; }

        /// <summary>
        /// Gets or sets the receiver value.
        /// </summary>
        /// <value>The receiver value.</value>
        [DataMember]
        public string ReceiverValue { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the life time.
        /// Unit: second.
        /// Max: 864000 Seconds (10 days).
        /// Default: 86400 Seconds (1 days).      
        /// </summary>
        /// <value>The life time.</value>
        [DataMember]
        public int LifeTime { get; set; }

        /// <summary>
        /// Gets or sets the override message unique identifier.
        /// </summary>
        /// <value>The override message unique identifier.</value>
        [DataMember]
        public string OverrideMessageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is test.
        /// For iOS only.
        /// Default: false.
        /// </summary>
        /// <value><c>true</c> if this instance is test; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool IsTestEnvironment { get; set; }

        /// <summary>
        /// Automatics the post dictionary.
        /// </summary>
        /// <returns>Dictionary{System.StringSystem.String}.</returns>
        public Dictionary<string, object> ToPostDictionary()
        {
            var result = new Dictionary<string, object>();

            // Mandatory parameters
            result.Add("receiver_type", (int)PushType);
            result.Add("msg_type", (int)MessageType);
            result.Add("msg_content", Message.ToJson(Platform));
            result.Add("platform", PlatformToString(Platform));
            if (Platform.Contains(PushPlatform.iOS))
            {
                result.Add("apns_production", IsTestEnvironment ? 0 : 1);
            }

            // Optional parameters
            if (!string.IsNullOrWhiteSpace(ReceiverValue))
            {
                result.Add("receiver_value", ReceiverValue);
            }
            if (!string.IsNullOrWhiteSpace(Description))
            {
                result.Add("send_description", Description);
            }
            if (LifeTime > 0)
            {
                result.Add("time_to_live", LifeTime > MaxLifeTime ? MaxLifeTime : LifeTime);
            }
            if (!string.IsNullOrWhiteSpace(OverrideMessageId))
            {
                result.Add("override_msg_id", OverrideMessageId);
            }

            return result;
        }

        /// <summary>
        /// Generates a platform string from enum.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>System.String.</returns>
        public static string PlatformToString(PushPlatform platform)
        {
            var list = new List<string>();
            if (platform.Contains(PushPlatform.Android))
            {
                list.Add(PushPlatform.Android.ToString().ToLowerInvariant());
            }
            if (platform.Contains(PushPlatform.iOS))
            {
                list.Add(PushPlatform.iOS.ToString().ToLowerInvariant());
            }

            return string.Join(",", list);
        }
    }
}
