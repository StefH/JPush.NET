using System.Collections.Generic;
using System.Runtime.Serialization;
using JPush.Extensions;
using RestSharp.Serializers;

namespace JPush.Models
{
    /// <summary>
    /// Class PushMessage.
    /// <see cref="PushMessageRequest"/> and <see cref="PushMessage"/> work together to send out push request.
    /// See official RESTful API: http://docs.jpush.cn/display/dev/Push+API+v2
    /// </summary>
    [DataContract]
    [KnownType(typeof(MessageType))]
    public class PushMessage
    {
        #region Constants

        protected const string PostKeyContent = "";

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the customized value.
        /// </summary>
        /// <value>The customized value.</value>
        [DataMember]
        public Dictionary<string, string> CustomizedValue { get; set; }

        #region iOS only

        /// <summary>
        /// Gets or sets the badge value.
        /// </summary>
        /// <value>The badge value.</value>
        [DataMember]
        public int BadgeValue { get; set; }

        /// <summary>
        /// Gets or sets the sound.
        /// For iOS only.
        /// </summary>
        /// <value>The sound.</value>
        [DataMember]
        public string Sound { get; set; }

        #endregion

        #region Android Only

        /// <summary>
        /// Gets or sets the push title.
        /// For Android Only.
        /// </summary>
        /// <value>The push title.</value>
        [DataMember]
        public string PushTitle { get; set; }

        /// <summary>
        /// Gets or sets the builder unique identifier.
        /// For Android Only.
        /// Default is 0. Valid value is 1-1000
        /// </summary>
        /// <value>The builder unique identifier.</value>
        [DataMember]
        public int BuilderId { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// To json.
        /// iOS Push Message example:
        /// <example>
        /// {"n_content":"通知内容", "n_extras":{"ios":{"badge":88, "sound":"happy"}, "user_param_1":"value1", "user_param_2":"value2"}}
        /// </example>
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>System.String.</returns>
        public string ToJson(PushPlatform platform = PushPlatform.AndroidAndiOS)
        {
            var result = new Dictionary<string, object>();
            var extra = new Dictionary<string, object>();

            result.Merge("n_content", Content);

            if (CustomizedValue != null)
            {
                foreach (var key in CustomizedValue.Keys)
                {
                    extra.Merge(key, CustomizedValue[key]);
                }
            }

            if (platform.Contains(PushPlatform.iOS))
            {
                var iOSDictionary = new Dictionary<string, object>
                {
                    {"badge", BadgeValue}
                };

                if (!string.IsNullOrWhiteSpace(Sound))
                {
                    CustomizedValue.Merge("sound", Sound);
                }

                extra.Merge("ios", iOSDictionary);
            }

            if (platform.Contains(PushPlatform.Android))
            {
                if (!string.IsNullOrWhiteSpace(PushTitle))
                {
                    result.Merge("n_title", PushTitle);
                }

                if (BuilderId > 0 && BuilderId <= 1000)
                {
                    result.Merge("n_builder_id", BuilderId);
                }
            }

            result.Add("n_extras", extra);

            var ser = new JsonSerializer();
            return ser.Serialize(result);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToJson();
        }
    }
}