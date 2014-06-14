using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JPush.Core;
using JPush.Core.Extensions;
using JPush.Core.ExternalModels;
using JPush.Core.InternalModels;
using JPush.V2.ExternalModels;
using JPush.V2.Implementation;
using RestSharp;

namespace JPush.V2
{
    /// <summary>
    /// Class JPushClientV2.
    /// <example>
    /// Here is a sample based on .NET console application.
    /// <code>
    /// <![CDATA[
    ///  class Program
    ///   {
    ///        static void Main(string[] args)
    ///        {
    ///            var appKey = "1234567890abcdef"; // Your App Key from JPush
    ///            var masterSecret = "1234567890abcdef"; // Your Master Secret from JPush
    ///
    ///            Dictionary<string, string> customizedValues = new Dictionary<string, string>();
    ///            customizedValues.Add("CK1", "CV1");
    ///            customizedValues.Add("CK2", "CV2");
    ///
    ///            JPushClient client = new JPushClient(appKey, masterSecret, false);
    ///            var response = client.SendPushMessage(new PushMessageRequest
    ///                       {
    ///                           MessageType = MessageType.Notification,
    ///                           Platform = PushPlatform.Android,
    ///                           Description = "DotNET",
    ///                           PushType = PushType.Broadcast,
    ///                           IsTestEnvironment = true,
    ///                           Message = new PushMessage
    ///                           {
    ///                               Content = "Hello, this is a test push from .NET. Have a nice day!",
    ///                               PushTitle = "A title.",
    ///                               Sound = "YourSound",
    ///                               CustomizedValue = customizedValues
    ///                           }
    ///                       });
    ///
    ///            Console.WriteLine(response.ResponseCode.ToString() + ":" + response.ResponseMessage);
    ///            Console.WriteLine("Push sent.");
    ///
    ///
    ///            List<string> idToCheck = new List<string>();
    ///            idToCheck.Add(response.MessageId);
    ///            var statusList = client.QueryPushMessageStatus(idToCheck);
    ///
    ///            Console.WriteLine("Status track is completed.");
    ///
    ///            if (statusList != null)
    ///            {
    ///                foreach (var one in statusList)
    ///                {
    ///                    Console.WriteLine("Id: {0}, Android: {1}, iOS: {2}", one.MessageId, one.AndroidDeliveredCount, one.ApplePushNotificationDeliveredCount);
    ///                }
    ///            }
    ///
    ///            Console.WriteLine("Press any key to exit.");
    ///            Console.Read();
    ///        }
    ///    }
    ///    ]]>
    ///    </code>
    /// </example>
    /// RESTful API reference: http://docs.jpush.cn/display/dev/Push+API+v2
    /// </summary>
    public class JPushClientV2 : JPushClientBase
    {
        /// <summary>
        /// Gets the API base URL.
        /// </summary>
        /// <value>The API base URL.</value>
        protected override string GetRemoteApiUrlFormat()
        {
            return "{0}:{1}/v2/";
        }

        /// <summary>
        /// Gets the Report base URL.
        /// </summary>
        /// <returns>The Report base URL.</returns>
        protected override string GetRemoteReportUrlFormat()
        {
            return "{0}:{1}/v2/";
        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="JPushClientV2" /> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <param name="useSSL">if set to <c>true</c> [use SSL].</param>
        /// <param name="proxySettings">The proxy settings.</param>
        public JPushClientV2(string appKey, string masterSecret, bool useSSL = true, JPushProxySettings proxySettings = null)
            : base(appKey, masterSecret, proxySettings)
        {
            string apiPrefix = useSSL ? "https://" : "http://";
            ApiHost = string.Format("{0}api.jpush.cn", apiPrefix);
            ApiPort = useSSL ? HttpsPort : HttpPort;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sends the push message.
        /// </summary>
        /// <param name="request">The PushMessageRequest.</param>
        /// <returns>PushResponse.</returns>
        public PushResponse SendPushMessage(PushMessageRequest request)
        {
            ValidatePushMessageRequest(request);

            var client = CreateJPushRestClient(ApiBaseUrl);
            var restRequest = CreatePushMessageRequest(request);

            var restResponse = client.Execute<JPushResponse>(restRequest);
            return Map(restResponse);
        }

        /// <summary>
        /// Sends the push message async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public Task<PushResponse> SendPushMessageAsync(PushMessageRequest request)
        {
            ValidatePushMessageRequest(request);

            var client = CreateJPushRestClient(ApiBaseUrl);
            var restRequest = CreatePushMessageRequest(request);

            var resttask = client.ExecuteTaskAsync<JPushResponse>(restRequest);
            return resttask.MapTask(Map);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Validates the push message request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        private void ValidatePushMessageRequest(PushMessageRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
        }

        /// <summary>
        /// Creates the push message request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>RestRequest</returns>
        private RestRequest CreatePushMessageRequest(PushMessageRequest request, int? timeout = 30 * 1000)
        {
            var restRequest = new RestRequest("push", Method.POST)
            {
                Timeout = timeout ?? 0
            };

            foreach (var kvp in CreatePostDictionary(request))
            {
                restRequest.AddParameter(kvp.Key, kvp.Value);
            }

            return restRequest;
        }

        /// <summary>
        /// Creates the query push message status request.
        /// </summary>
        /// <param name="messageIdCollection">The message identifier collection.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>RestRequest</returns>
        protected override RestRequest CreateQueryPushMessageStatusRequest(List<string> messageIdCollection, int? timeout = 30 * 1000)
        {
            var restRequest = new RestRequest("received", Method.GET)
            {
                Timeout = timeout ?? 0
            };

            restRequest.AddParameter("msg_ids", string.Join(",", messageIdCollection.Take(MaxQueryIds)));

            return restRequest;
        }

        /// <summary>
        /// Generates the verification code.
        /// </summary>
        /// <param name="sendIdentity">The send identity.</param>
        /// <param name="receiverType">Type of the receiver.</param>
        /// <param name="receiverValue">The receiver value.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <returns>System.String.</returns>
        private string GenerateVerificationCode(int sendIdentity, int receiverType, string receiverValue, string masterSecret)
        {
            return string.Format("{0}{1}{2}{3}", sendIdentity, receiverType, receiverValue, masterSecret).ToMD5();
        }

        /// <summary>
        /// Creates the post dictionary.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Dictionary{System.StringSystem.String}.</returns>
        private Dictionary<string, object> CreatePostDictionary(PushMessageRequest request)
        {
            if (request == null)
            {
                return null;
            }

            var result = request.ToPostDictionary();
            var sendIdentity = GenerateSendIdentity();

            result.Merge("sendno", sendIdentity);
            result.Merge("app_key", AppKey);
            result.Merge("verification_code", GenerateVerificationCode(sendIdentity, (int)request.PushType, request.ReceiverValue, MasterSecret));

            return result;
        }
        #endregion
    }
}