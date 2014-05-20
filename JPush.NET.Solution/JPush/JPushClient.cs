using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JPush.Extensions;
using JPush.JPushModels;
using JPush.JPushRest;
using JPush.Models;
using RestSharp;

namespace JPush
{
    /// <summary>
    /// Class JPushClient.
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
    /// RESTful API reference: http://docs.jpush.cn/display/dev/Index
    /// </summary>
    public class JPushClient
    {
        #region Data

        /// <summary>
        /// JPush has limitation officially. One query support no more than 100 IDs.
        /// </summary>
        protected const int MaxQueryIds = 100;

        /// <summary>
        /// The HTTP port
        /// </summary>
        protected const int HttpPort = 8800;

        /// <summary>
        /// The HTTPS port
        /// </summary>
        protected const int HttpsPort = 443;

        /// <summary>
        /// The remote base URL format
        /// </summary>
        protected const string RemoteBaseUrlFormat = "{0}:{1}/v2/";

        /// <summary>
        /// The port
        /// </summary>
        protected int ApiPort;

        /// <summary>
        /// The report port
        /// </summary>
        protected int ReportPort;

        /// <summary>
        /// The host
        /// </summary>
        protected string ApiHost;

        /// <summary>
        /// The report host
        /// </summary>
        protected string ReportHost;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        /// <value>The application key.</value>
        public string AppKey
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the master secret.
        /// </summary>
        /// <value>The master secret.</value>
        public string MasterSecret
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the API base URL.
        /// </summary>
        /// <value>The API base URL.</value>
        public string ApiBaseUrl
        {
            get
            {
                return string.Format(RemoteBaseUrlFormat, ApiHost, ApiPort);
            }
        }

        /// <summary>
        /// Gets the Report base URL.
        /// </summary>
        /// <value>The Report base URL.</value>
        public string ReportBaseUrl
        {
            get
            {
                return string.Format(RemoteBaseUrlFormat, ReportHost, ReportPort);
            }
        }

        /// <summary>
        /// The ProxySettings
        /// </summary>
        protected readonly JPushProxySettings ProxySettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JPushClient" /> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <param name="useSSL">if set to <c>true</c> [use SSL].</param>
        /// <param name="proxySettings">The proxy settings.</param>
        public JPushClient(string appKey, string masterSecret, bool useSSL = true, JPushProxySettings proxySettings = null)
        {
            AppKey = appKey;
            MasterSecret = masterSecret;

            string apiPrefix = useSSL ? "https://" : "http://";
            ApiHost = string.Format("{0}api.jpush.cn", apiPrefix);
            ApiPort = useSSL ? HttpsPort : HttpPort;

            ReportHost = "https://report.jpush.cn";
            ReportPort = HttpsPort;

            ProxySettings = proxySettings;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sends the push message.
        /// </summary>
        /// <param name="request">The PushMessageRequest.</param>
        /// <returns>PushResponse.</returns>
        public PushResponse SendPushMessage(PushMessageRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

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
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var client = CreateJPushRestClient(ApiBaseUrl);
            var restRequest = CreatePushMessageRequest(request);

            var resttask = client.ExecuteTaskAsync<JPushResponse>(restRequest);
            return resttask.MapTask(Map);
        }

        /// <summary>
        /// Queries the push message status.
        /// JPush has limitation offically. One query support no more than 100 IDs. So if the input has more than 100 IDs, only the first 100 IDs would be queried.
        /// </summary>
        /// <param name="messageIdCollection">The message unique identifier collection.</param>
        /// <returns>List{PushMessageStatus}.</returns>
        /// <exception cref="System.InvalidOperationException">Failed to QueryPushMessageStatus.</exception>
        public List<PushMessageStatus> QueryPushMessageStatus(List<string> messageIdCollection)
        {
            if (messageIdCollection == null)
            {
                throw new ArgumentNullException("messageIdCollection");
            }

            var client = CreateJPushRestClient(ReportBaseUrl);
            var restRequest = CreateQueryPushMessageStatusRequest(messageIdCollection);

            var restResponse = client.Execute<List<JPushMessageStatusResponse>>(restRequest);
            return Map(restResponse);
        }

        /// <summary>
        /// Queries the push message status async.
        /// JPush has limitation offically. One query support no more than 100 IDs. So if the input has more than 100 IDs, only the first 100 IDs would be queried.
        /// </summary>
        /// <param name="messageIdCollection">The message id collection.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">messageIdCollection</exception>
        public Task<List<PushMessageStatus>> QueryPushMessageStatusAsync(List<string> messageIdCollection)
        {
            if (messageIdCollection == null)
            {
                throw new ArgumentNullException("messageIdCollection");
            }

            var client = CreateJPushRestClient(ReportBaseUrl);
            var restRequest = CreateQueryPushMessageStatusRequest(messageIdCollection);

            var resttask = client.ExecuteTaskAsync<List<JPushMessageStatusResponse>>(restRequest);
            return resttask.MapTask(Map);
        }

        /// <summary>
        /// Overrides the API URL. (Can be used for testing purposes)
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public void OverrideApiUrl(string host, int port)
        {
            ApiHost = host;
            ApiPort = port;
        }

        /// <summary>
        /// Overrides the report URL. (Can be used for testing purposes)
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public void OverrideReportUrl(string host, int port)
        {
            ReportHost = host;
            ReportPort = port;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Creates the query push message status request.
        /// </summary>
        /// <param name="messageIdCollection">The message identifier collection.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>RestRequest</returns>
        protected RestRequest CreateQueryPushMessageStatusRequest(List<string> messageIdCollection, int? timeout = 30 * 1000)
        {
            var restRequest = new RestRequest("received", Method.GET)
            {
                Timeout = timeout ?? 0
            };

            restRequest.AddParameter("msg_ids", string.Join(",", messageIdCollection.Take(MaxQueryIds)));

            return restRequest;
        }

        /// <summary>
        /// Creates the push message request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>RestRequest</returns>
        protected RestRequest CreatePushMessageRequest(PushMessageRequest request, int? timeout = 30 * 1000)
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

        protected JPushRestClient CreateJPushRestClient(string baseUrl)
        {
            var client = new JPushRestClient(baseUrl, AppKey, MasterSecret);
            if (ProxySettings != null && ProxySettings.Enabled)
            {
                client.Proxy = ProxySettings.GetWebProxy();
            }

            return client;
        }

        /// <summary>
        /// Maps a IRestResponse[JPushResponse] to PushResponse
        /// </summary>
        /// <param name="restReponse">The IRestResponse[JPushResponse].</param>
        /// <returns>PushResponse</returns>
        protected PushResponse Map(IRestResponse<JPushResponse> restReponse)
        {
            if (restReponse == null)
            {
                return new PushResponse
                {
                    ResponseCode = PushResponseCode.ServiceError,
                    ResponseMessage = "The RestReponse is null"
                };
            }

            switch (restReponse.ResponseStatus)
            {
                case ResponseStatus.Completed:
                    var result = new PushResponse
                    {
                        ResponseCode = (PushResponseCode)restReponse.Data.ErrorCode,
                        ResponseMessage = restReponse.Data.ErrorMessage
                    };

                    if (result.ResponseCode == PushResponseCode.Succeed)
                    {
                        result.MessageId = restReponse.Data.MessageId;
                        result.SendIdentity = restReponse.Data.SendNo;
                    }

                    return result;

                default:
                    return new PushResponse
                    {
                        ResponseCode = PushResponseCode.ServiceError,
                        ResponseMessage = restReponse.ErrorMessage
                    };
            }
        }

        /// <summary>
        /// Maps IRestResponse[List[JPushMessageStatusResponse]] to List[PushMessageStatus].
        /// </summary>
        /// <param name="restResponse">The IRestResponse[List[JPushMessageStatusResponse]].</param>
        /// <returns>List[PushMessageStatus]</returns>
        protected List<PushMessageStatus> Map(IRestResponse<List<JPushMessageStatusResponse>> restResponse)
        {
            if (restResponse == null)
            {
                return null;
            }

            switch (restResponse.ResponseStatus)
            {
                case ResponseStatus.Completed:
                    return restResponse.Data
                       .Select(j => new PushMessageStatus
                       {
                           MessageId = j.MessageId,
                           AndroidDeliveredCount = j.AndroidReceived,
                           ApplePushNotificationDeliveredCount = j.ApplePushNotificationSent
                       })
                       .ToList();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Maps a PushMessageRequest to a JPushMessageRequest.
        /// </summary>
        /// <param name="request">The PushMessageRequest.</param>
        /// <returns>A JPushMessageRequest</returns>
        protected JPushMessageRequest Map(PushMessageRequest request)
        {
            var sendIdentity = GenerateSendIdentity();

            return new JPushMessageRequest
            {
                AppKey = AppKey,
                Description = request.Description,
                LifeTime = request.LifeTime > 0 ? request.LifeTime : (int?)null,
                MessageContent = request.Message.ToJson(request.Platform),
                MessageType = (int)request.MessageType,
                OverrideMessageId = request.OverrideMessageId,
                Platform = PushMessageRequest.PlatformToString(request.Platform),
                ReceiverType = (int)request.PushType,
                ReceiverValue = request.ReceiverValue,
                SendNo = sendIdentity,
                VerificationCode = GenerateVerificationCode(sendIdentity, (int)request.PushType, request.ReceiverValue, MasterSecret)
            };
        }

        /// <summary>
        /// Creates the post dictionary.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Dictionary{System.StringSystem.String}.</returns>
        protected Dictionary<string, object> CreatePostDictionary(PushMessageRequest request)
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

        /// <summary>
        /// Generates the verification code.
        /// </summary>
        /// <param name="sendIdentity">The send identity.</param>
        /// <param name="receiverType">Type of the receiver.</param>
        /// <param name="receiverValue">The receiver value.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <returns>System.String.</returns>
        protected static string GenerateVerificationCode(int sendIdentity, int receiverType, string receiverValue, string masterSecret)
        {
            return string.Format("{0}{1}{2}{3}", sendIdentity, receiverType, receiverValue, masterSecret).ToMD5();
        }

        /// <summary>
        /// Generates the query token.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <returns>System.String.</returns>
        protected static string GenerateQueryToken(string appKey, string masterSecret)
        {
            return string.Format("{0}:{1}", appKey, masterSecret).ToBase64();
        }

        /// <summary>
        /// Generates the send identity.
        /// The total milliseconds value of offset from UTC now to UTC 2014 Jan 1st.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected static int GenerateSendIdentity()
        {
            return (int)(((DateTime.UtcNow - new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds) % Int32.MaxValue);
        }

        #endregion
    }
}