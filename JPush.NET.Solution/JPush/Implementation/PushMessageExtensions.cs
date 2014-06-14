using System;
using System.Collections.Generic;
using JPush.Core.Extensions;
using JPush.V2.ExternalModels;
using RestSharp.Serializers;

namespace JPush.V2.Implementation
{
    public static class PushMessageExtensions
    {
        /// <summary>
        /// To json.
        /// iOS Push Message example:
        /// <example>
        /// {"n_content":"通知内容", "n_extras":{"ios":{"badge":88, "sound":"happy"}, "user_param_1":"value1", "user_param_2":"value2"}}
        /// </example>
        /// </summary>
        /// <param name="pushMessage">The pushMessage.</param>
        /// <param name="platform">The platform.</param>
        /// <returns>System.String.</returns>
        public static string ToJson(this PushMessage pushMessage, PushPlatform platform = PushPlatform.AndroidAndIOS)
        {
            var result = new Dictionary<string, object>();
            var extra = new Dictionary<string, object>();

            result.Merge("n_content", pushMessage.Content);

            if (pushMessage.CustomizedValue != null)
            {
                foreach (var key in pushMessage.CustomizedValue.Keys)
                {
                    extra.Merge(key, pushMessage.CustomizedValue[key]);
                }
            }

            if (platform.Contains(PushPlatform.IOS))
            {
                var iOSDictionary = new Dictionary<string, object>
                {
                    {"badge", pushMessage.BadgeValue}
                };

                if (!String.IsNullOrWhiteSpace(pushMessage.Sound))
                {
                    pushMessage.CustomizedValue.Merge("sound", pushMessage.Sound);
                }

                extra.Merge("ios", iOSDictionary);
            }

            if (platform.Contains(PushPlatform.Android))
            {
                if (!String.IsNullOrWhiteSpace(pushMessage.PushTitle))
                {
                    result.Merge("n_title", pushMessage.PushTitle);
                }

                if (pushMessage.BuilderId > 0 && pushMessage.BuilderId <= 1000)
                {
                    result.Merge("n_builder_id", pushMessage.BuilderId);
                }
            }

            result.Add("n_extras", extra);

            var ser = new JsonSerializer();
            return ser.Serialize(result);
        }

        /// <summary>
        /// Determines whether [contains] [the specified platform value].
        /// </summary>
        /// <param name="platformValue">The platform value.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [contains] [the specified platform value]; otherwise, <c>false</c>.</returns>
        public static bool Contains(this PushPlatform platformValue, PushPlatform value)
        {
            return ((int)platformValue & (int)value) > 0;
        }
    }
}