using System;
using System.Collections.Specialized;
using JPush.V2.InternalModels;
using JPush.V3.ExternalModels;

namespace JPush.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var testServer = new JPushTestServer("http://localhost:10000")
            {
                PushMessageReceivedV2 = PushMessageReceivedV2,
                PushMessageReceivedV3 = PushMessageReceivedV3,
                QueryPushMessageStatusReceived = QueryPushMessageStatusReceived
            };

            testServer.Start();

            Console.WriteLine("JPushTestServer running. Press any key to stop.");
            Console.ReadKey();

            testServer.Stop();
            Console.WriteLine("JPushTestServer stopped.");
        }

        private static void PushMessageReceivedV2(NameValueCollection queryValues, JPushMessageRequest request)
        {
            Console.WriteLine("POST /v2/push");

            foreach (string key in queryValues)
            {
                Console.WriteLine("[{0}]={1}", key, queryValues[key]);
            }
        }

        private static void PushMessageReceivedV3(JPushMessage message)
        {
            Console.WriteLine("POST /v3/push");

            Console.WriteLine(message);
        }

        private static void QueryPushMessageStatusReceived(string messageIds)
        {
            Console.WriteLine("GET /v2/received");
            Console.WriteLine("[msg_ids]={0}", messageIds);
        }
    }
}