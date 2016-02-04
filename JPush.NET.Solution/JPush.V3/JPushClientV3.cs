using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JPush.Core;
using JPush.Core.Extensions;
using JPush.Core.ExternalModels;
using JPush.Core.InternalModels;
using JPush.Core.JPushRest;
using JPush.V3.ExternalModels;
using RestSharp;

namespace JPush.V3
{
    /// <summary>
    /// RESTful API reference: http://docs.jpush.cn/display/dev/Push-API-v3
    /// </summary>
    public class JPushClientV3 : JPushClientBase
    {
        #region Delegate Definitions
        /// <summary>
        /// Delegate for PushMessageSent
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="notification">The notification.</param>
        /// <param name="response">The response.</param>
        public delegate void PushMessageSentDelegate(object sender, JPushMessage notification, PushResponse response);

        /// <summary>
        /// Delegate for PushMessageFailed (Exception or Timeout)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="notification">The notification.</param>
        /// <param name="exception">The exception.</param>
        public delegate void PushMessageFailedDelegate(object sender, JPushMessage notification, Exception exception);
        #endregion

        #region Delegates
        public event PushMessageSentDelegate OnPushMessageSent;

        public event PushMessageFailedDelegate OnPushMessageFailed;
        #endregion

        /// <summary>
        /// Gets the API base URL.
        /// </summary>
        /// <value>The API base URL.</value>
        protected override string GetRemoteApiUrlFormat()
        {
            return "{0}:{1}/v3/";
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
        /// Initializes a new instance of the <see cref="JPushClientV3" /> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <param name="proxySettings">The proxy settings.</param>
        public JPushClientV3(string appKey, string masterSecret, JPushProxySettings proxySettings = null)
            : base(appKey, masterSecret, proxySettings)
        {
            ApiHost = "https://api.jpush.cn";
            ApiPort = HttpsPort;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Sends the push message.
        /// </summary>
        /// <param name="message">The PushMessageRequest.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns>PushResponse </returns>
        public PushResponse SendPushMessage(JPushMessage message, int? timeout = 30 * 1000)
        {
            ValidateSendPushMessage(message);

            var client = CreateJPushRestClient(ApiBaseUrl, new RestSharpJsonNetDeserializer());
            var restRequest = CreatePushMessageRequest(message, timeout);

            var restResponse = client.Execute<JPushResponse>(restRequest);
            return Map(restResponse);
        }

        /// <summary>
        /// Sends the push message async.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <returns>Task PushResponse</returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public Task<PushResponse> SendPushMessageAsync(JPushMessage message, int? timeout = 30 * 1000)
        {
            ValidateSendPushMessage(message);

            var client = CreateJPushRestClient(ApiBaseUrl, new RestSharpJsonNetDeserializer());
            var restRequest = CreatePushMessageRequest(message, timeout);

            var resttask = client.ExecuteTaskAsync<JPushResponse>(restRequest);
            return resttask.MapTask(Map);
        }

        /// <summary>
        /// Queues the push message async.
        /// TODO : implement real queue
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="timeout">The timeout in milliseconds..</param>
        public void QueuePushMessageAsync(JPushMessage message, int? timeout = 30 * 1000)
        {
            SendPushMessageAsync(message, timeout)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        if (OnPushMessageFailed != null)
                        {
                            OnPushMessageFailed(this, message, task.Exception);
                        }
                    }
                    else if (task.IsCanceled)
                    {
                        if (OnPushMessageFailed != null)
                        {
                            OnPushMessageFailed(this, message, new TaskCanceledException());
                        }
                    }
                    else
                    {
                        if (OnPushMessageSent != null)
                        {
                            OnPushMessageSent(this, message, task.Result);
                        }
                    }
                });
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Validates the JPushMessage.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException">message</exception>
        /// <exception cref="System.ArgumentException"></exception>
        private void ValidateSendPushMessage(JPushMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.Audience == null)
            {
                throw new ArgumentException("message.Audience");
            }

            var audience = message.Audience as Audience;
            if (audience != null)
            {
                if (audience.Tag != null && audience.Tag.Count > Audience.MaxTag)
                {
                    throw new ArgumentException(string.Format("message.Audience.Tag Count > {0}", Audience.MaxTag));
                }

                if (audience.TagAnd != null && audience.TagAnd.Count > Audience.MaxTagAnd)
                {
                    throw new ArgumentException(string.Format("message.Audience.MaxTagAnd Count > {0}", Audience.MaxTagAnd));
                }

                if (audience.Alias != null && audience.Alias.Count > Audience.MaxAlias)
                {
                    throw new ArgumentException(string.Format("message.Audience.MaxAlias Count > {0}", Audience.MaxAlias));
                }

                if (audience.RegistrationId != null && audience.RegistrationId.Count > Audience.MaxRegistrationId)
                {
                    throw new ArgumentException(string.Format("message.Audience.MaxRegistrationId Count > {0}", Audience.MaxRegistrationId));
                }
            }

            if (message.Options != null)
            {
                if (message.Options.Ttl > Options.MaxLifeTime)
                {
                    throw new ArgumentException(string.Format("message.Options.TimeToLive > {0}", Options.MaxLifeTime));
                }
            }
        }

        ///<summary>
        /// Creates the push message request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="timeout">The timeout in milliseconds to be used for the request.</param>
        /// <returns>RestRequest</returns>
        private RestRequest CreatePushMessageRequest(JPushMessage message, int? timeout)
        {
            var restRequest = new RestRequest("push", Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            if (timeout.HasValue)
            {
                restRequest.Timeout = timeout.Value;
            }

            restRequest.AddBody(message);

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
        #endregion
    }
}