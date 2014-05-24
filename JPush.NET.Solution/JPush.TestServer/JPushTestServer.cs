using System;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Web;
using JPush.JPushModels;

namespace JPush.TestServer
{
    public delegate void PushMessageReceivedDelegate(NameValueCollection queryValues, JPushMessageRequest request);
    public delegate void QueryPushMessageStatusReceivedDelegate(string messageIds);

    public class JPushTestServer
    {
        private readonly WebServiceHost _serviceHost;
        private readonly JPushServices _services;

        private PushMessageReceivedDelegate _pushMessageReceived;
        private QueryPushMessageStatusReceivedDelegate _queryPushMessageStatusReceived;

        public PushMessageReceivedDelegate PushMessageReceived
        {
            set
            {
                _pushMessageReceived = value;
                _services.PushMessageReceived = _pushMessageReceived;
            }
        }

        public QueryPushMessageStatusReceivedDelegate QueryPushMessageStatusReceived
        {
            set
            {
                _queryPushMessageStatusReceived = value;
                _services.QueryPushMessageStatusReceived = _queryPushMessageStatusReceived;
            }
        }

        public JPushTestServer(string baseAddress)
        {
            _services = new JPushServices();
            _serviceHost = new WebServiceHost(_services, new Uri(string.Format("{0}/v2/", baseAddress)));

            var binding = new WebHttpBinding();
            _serviceHost.AddServiceEndpoint(typeof(JPushServices), binding, "");
        }

        public void Start()
        {
            _serviceHost.Open();
        }

        public void Stop()
        {
            _serviceHost.Close();
        }
    }
}