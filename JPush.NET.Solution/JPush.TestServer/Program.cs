using System;
using System.Collections.Specialized;
using JPush.JPushModels;

namespace JPush.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var testServer = new JPushTestServer("http://localhost:10000")
            {
                PushMessageReceived = PushMessageReceived,
                QueryPushMessageStatusReceived = QueryPushMessageStatusReceived
            };

            testServer.Start();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            testServer.Stop();
        }

        private static void PushMessageReceived(NameValueCollection queryValues, JPushMessageRequest request)
        {
            Console.WriteLine("POST /push");

            foreach (string key in queryValues)
            {
                Console.WriteLine("[{0}]={1}", key, queryValues[key]);
            }
        }

        private static void QueryPushMessageStatusReceived(string messageIds)
        {
            Console.WriteLine("GET /received");
            Console.WriteLine("[msg_ids]={0}", messageIds);
        }
    }
}
