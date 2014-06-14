using System.Collections.Generic;
using JPush.V2.ExternalModels;

namespace JPush.V2.Implementation
{
    public static class PushMessageRequestExtensions
    {
        /// <summary>
        /// Automatics the post dictionary.
        /// </summary>
        /// <returns>Dictionary{System.StringSystem.String}.</returns>
        public static Dictionary<string, object> ToPostDictionary(this PushMessageRequest request)
        {
            var result = new Dictionary<string, object>();

            // Mandatory parameters request
            result.Add("receiver_type", (int)request.PushType);
            result.Add("msg_type", (int)request.MessageType);
            result.Add("msg_content", request.Message.ToJson(request.Platform));
            result.Add("platform", request.Platform.AsString());
            if (request.Platform.Contains(PushPlatform.IOS))
            {
                result.Add("apns_production", request.IsTestEnvironment ? 0 : 1);
            }

            // Optional parameters
            if (!string.IsNullOrWhiteSpace(request.ReceiverValue))
            {
                result.Add("receiver_value", request.ReceiverValue);
            }
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                result.Add("send_description", request.Description);
            }
            if (request.LifeTime > 0)
            {
                result.Add("time_to_live", request.LifeTime > PushMessageRequest.MaxLifeTime ? PushMessageRequest.MaxLifeTime : request.LifeTime);
            }
            if (!string.IsNullOrWhiteSpace(request.OverrideMessageId))
            {
                result.Add("override_msg_id", request.OverrideMessageId);
            }

            return result;
        }

        /// <summary>
        /// Generates a platform string from enum.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>System.String.</returns>
        public static string AsString(this PushPlatform platform)
        {
            var list = new List<string>();
            if (platform.Contains(PushPlatform.Android))
            {
                list.Add(PushPlatform.Android.ToString().ToLowerInvariant());
            }
            if (platform.Contains(PushPlatform.IOS))
            {
                list.Add(PushPlatform.IOS.ToString().ToLowerInvariant());
            }

            return string.Join(",", list);
        }
    }
}
