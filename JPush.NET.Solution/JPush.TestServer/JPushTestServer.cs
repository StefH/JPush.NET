using System;
using System.Collections.Specialized;
using System.ServiceModel;
using System.ServiceModel.Web;
using JPush.V2.InternalModels;
using JPush.V3.ExternalModels;

namespace JPush.TestServer
{
    public delegate void PushMessageReceivedV2Delegate(NameValueCollection queryValues, JPushMessageRequest request);
    public delegate void PushMessageReceivedV3Delegate(JPushMessage message);
    public delegate void QueryPushMessageStatusReceivedDelegate(string messageIds);

    public class JPushTestServer
    {
        private readonly WebServiceHost _serviceHostV2;
        private readonly WebServiceHost _serviceHostV3;

        private readonly JPushServiceV2 _serviceV2 = new JPushServiceV2();
        private readonly JPushServiceV3 _serviceV3 = new JPushServiceV3();

        private PushMessageReceivedV2Delegate _pushMessageReceivedV2;
        private PushMessageReceivedV3Delegate _pushMessageReceivedV3;
        private QueryPushMessageStatusReceivedDelegate _queryPushMessageStatusReceived;

        public PushMessageReceivedV2Delegate PushMessageReceivedV2
        {
            set
            {
                _pushMessageReceivedV2 = value;
                _serviceV2.PushMessageReceived = _pushMessageReceivedV2;
            }
        }

        public PushMessageReceivedV3Delegate PushMessageReceivedV3
        {
            set
            {
                _pushMessageReceivedV3 = value;
                _serviceV3.PushMessageReceived = _pushMessageReceivedV3;
            }
        }

        public QueryPushMessageStatusReceivedDelegate QueryPushMessageStatusReceived
        {
            set
            {
                _queryPushMessageStatusReceived = value;
                _serviceV2.QueryPushMessageStatusReceived = _queryPushMessageStatusReceived;
            }
        }

        public JPushTestServer(string baseAddress)
        {
            _serviceHostV2 = new WebServiceHost(_serviceV2, new Uri(string.Format("{0}/v2/", baseAddress)));
            _serviceHostV3 = new WebServiceHost(_serviceV3, new Uri(string.Format("{0}/v3/", baseAddress)));

            var bindingV2 = new WebHttpBinding();
            _serviceHostV2.AddServiceEndpoint(typeof(JPushServiceV2), bindingV2, "");

            var bindingV3 = new WebHttpBinding();
            _serviceHostV3.AddServiceEndpoint(typeof(JPushServiceV3), bindingV3, "");
        }

        public void Start()
        {
            _serviceHostV2.Open();
            _serviceHostV3.Open();
        }

        public void Stop()
        {
            _serviceHostV2.Close();
            _serviceHostV3.Close();
        }
    }
}