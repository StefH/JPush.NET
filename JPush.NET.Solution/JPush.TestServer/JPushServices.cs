using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using JPush.JPushModels;
using JPushTestServer.Extensions;

namespace JPushTestServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract(Name = "JPushServices")]
    public class JPushServices
    {
        // GET /v2/received
        // msg_ids=1,2,3
        [OperationContract]
        [WebGet(UriTemplate = "received?msg_ids={messageIds}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<JPushMessageStatusResponse> QueryPushMessageStatus(string messageIds)
        {
            Console.WriteLine("GET /received");
            Console.WriteLine("[msg_ids]={0}", messageIds);

            return messageIds.Split(',').Select(messageId =>
                new JPushMessageStatusResponse
                {
                    MessageId = messageId,
                    AndroidReceived = 1,
                    ApplePushNotificationSent = 0
                }
            );
        }

        // POST /v2/push
        // receiver_type=5&
        // msg_type=1&
        // msg_content=%7B%22n_content%22%3A%22JPush%20ByRegistrationId%20message%20%40%203%3A26%3A36%20PM%22%2C%22n_title%22%3A%22ASYNC%20%3A%20%3C%E6%8E%A8%E9%80%81%E6%9C%8D%E5%8A%A1%3E%22%2C%22n_extras%22%3A%7B%7D%7D&
        // platform=android&
        // receiver_value=040f2c4faa0&
        // send_description=DotNET&
        // sendno=629378330&
        // app_key=123&
        // verification_code=456
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "push", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public JPushResponse PushMessage(Stream stream)
        {
            Console.WriteLine("POST /push");
            var data = new StreamReader(stream).ReadToEnd();

            var queryValues = HttpUtility.ParseQueryString(data);
            foreach (string key in queryValues)
            {
                Console.WriteLine("[{0}]={1}", key, queryValues[key]);
            }

            var request = ParseJPushMessageRequest(queryValues);

            return new JPushResponse
            {
                ErrorCode = 0,
                ErrorMessage = "",
                MessageId = string.Format("{0}{1}", DateTime.Now.Millisecond, DateTime.Now.Millisecond),
                SendNo = Convert.ToString(request.SendNo)
            };
        }

        private JPushMessageRequest ParseJPushMessageRequest(NameValueCollection queryValues)
        {
            var request = new JPushMessageRequest();
            var properties = request.GetType()
                   .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Select(p => new { Property = p, Attribute = p.GetCustomAttribute<DataMemberAttribute>() })
                   .ToList()
                   ;

            foreach (string key in queryValues)
            {
                var value = queryValues[key];
                var prop = properties.FirstOrDefault(p => p.Attribute.Name == key);
                if (prop != null)
                {
                    prop.Property.SetValue(request, Convert.ChangeType(value, prop.Property.PropertyType), null);
                }
            }

            return request;
        }
    }
}