using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using JPush.Core.InternalModels;
using JPush.V3.ExternalModels;

namespace JPush.TestServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract(Name = "JPushServiceV3")]
    public class JPushServiceV3
    {
        public PushMessageReceivedV3Delegate PushMessageReceived { get; set; }

        // POST /v3/push
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "push", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public JPushResponse PushMessage(JPushMessage message)
        {
            //var data = new StreamReader(stream).ReadToEnd();

            //var queryValues = HttpUtility.ParseQueryString(data);
            //var request = ParseJPushMessageRequest(queryValues);

           

            return new JPushResponse
            {
                ErrorCode = 0,
                ErrorMessage = "",
                MessageId = string.Format("{0}{1}", DateTime.Now.Millisecond, DateTime.Now.Millisecond),
                SendNo = Convert.ToString(message.Options != null ? message.Options.SendNo : null)
            };
        }
    }
}