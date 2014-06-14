using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JPush.Core.Extensions;
using JPush.Core.ExternalModels;
using JPush.Core.InternalModels;
using JPush.Core.JPushRest;
using RestSharp;
using RestSharp.Deserializers;

namespace JPush.Core
{
    public abstract class JPushClientBase
    {
        #region Constants
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
        #endregion

        #region Settings
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

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        /// <value>The application key.</value>
        protected string AppKey;

        /// <summary>
        /// Gets or sets the master secret.
        /// </summary>
        /// <value>The master secret.</value>
        protected string MasterSecret;

        /// <summary>
        /// Gets the API base URL.
        /// </summary>
        /// <value>The API base URL.</value>
        protected string ApiBaseUrl
        {
            get
            {
                return string.Format(GetRemoteApiUrlFormat(), ApiHost, ApiPort);
            }
        }

        /// <summary>
        /// Gets the Report base URL.
        /// </summary>
        /// <value>The Report base URL.</value>
        protected string ReportBaseUrl
        {
            get
            {
                return string.Format(GetRemoteReportUrlFormat(), ReportHost, ReportPort);
            }
        }

        /// <summary>
        /// The ProxySettings
        /// </summary>
        protected JPushProxySettings ProxySettings;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="JPushClientBase" /> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <param name="proxySettings">The proxy settings (can be null).</param>
        protected JPushClientBase(string appKey, string masterSecret, JPushProxySettings proxySettings = null)
        {
            AppKey = appKey;
            MasterSecret = masterSecret;

            ReportHost = "https://report.jpush.cn";
            ReportPort = HttpsPort;

            ProxySettings = proxySettings;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Queries the push message status.
        /// JPush has limitation offically. One query support no more than 100 IDs. So if the input has more than 100 IDs, only the first 100 IDs would be queried.
        /// </summary>
        /// <param name="messageIdCollection">The message unique identifier collection.</param>
        /// <returns>List{PushMessageStatus}.</returns>
        /// <exception cref="System.InvalidOperationException">Failed to QueryPushMessageStatus.</exception>
        public List<PushMessageStatus> QueryPushMessageStatus(List<string> messageIdCollection)
        {
            ValidateQueryPushMessageStatus(messageIdCollection);

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
            ValidateQueryPushMessageStatus(messageIdCollection);

            var client = CreateJPushRestClient(ReportBaseUrl);
            var restRequest = CreateQueryPushMessageStatusRequest(messageIdCollection);

            var resttask = client.ExecuteTaskAsync<List<JPushMessageStatusResponse>>(restRequest);
            return resttask.MapTask(Map);
        }

        /// <summary>
        /// Validates QueryPushMessageStatus request.
        /// </summary>
        /// <param name="messageIdCollection">The message identifier collection.</param>
        /// <exception cref="System.ArgumentNullException">messageIdCollection</exception>
        /// <exception cref="System.ArgumentException">messageIdCollection has one or more empty values</exception>
        private void ValidateQueryPushMessageStatus(IEnumerable<string> messageIdCollection)
        {
            if (messageIdCollection == null)
            {
                throw new ArgumentNullException("messageIdCollection");
            }

            if (messageIdCollection.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException("messageIdCollection has one or more empty values");
            }
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

        #region Abstract methods
        /// <summary>
        /// Get the remote base URL format "{0}:{1}/v2/"
        /// </summary>
        protected abstract string GetRemoteApiUrlFormat();

        /// <summary>
        /// Get the remote base URL format "{0}:{1}/v2/"
        /// </summary>
        protected abstract string GetRemoteReportUrlFormat();

        /// <summary>
        /// Creates the query push message status request.
        /// </summary>
        /// <param name="messageIdCollection">The message identifier collection.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>RestRequest</returns>
        protected abstract RestRequest CreateQueryPushMessageStatusRequest(List<string> messageIdCollection, int? timeout = 30*1000);
        #endregion

        #region Helper methods
        protected JPushRestClient CreateJPushRestClient(string baseUrl, IDeserializer deserializer = null)
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
        /// Generates the send identity.
        /// The total milliseconds value of offset from UTC now to UTC 2014 Jan 1st.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GenerateSendIdentity()
        {
            return (int)(((DateTime.UtcNow - new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds) % Int32.MaxValue);
        }
        #endregion
    }
}