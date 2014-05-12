using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JPush;
using JPush.Models;

namespace JPushConsoleExample
{
    class Program
    {
        private static string appKey; // Your App Key from JPush
        private static string masterSecret; // Your Master Secret from JPush

        static void Main(string[] args)
        {
            ReadIniFile();

            var client = new JPushClient(appKey, masterSecret, false);
            //client.OverrideApiUrl("http://localhost", 10000);
            //client.OverrideReportUrl("http://localhost", 10001);

            SendBroadcastMessage(client);
            SendByRegistrationIdMessage(client);

            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }

        private static void SendByRegistrationIdMessage(JPushClient client)
        {
            var request = new PushMessageRequest
           {
               MessageType = MessageType.Notification,
               Platform = PushPlatform.Android,
               Description = "DotNET",
               PushType = PushType.ByRegistrationId,
               ReceiverValue = "040f2c4faa0",
               IsTestEnvironment = true,
               Message = new PushMessage
               {
                   Content = "JPush ByRegistrationId message @ " + DateTime.Now.ToLongTimeString(),
                   PushTitle = "ASYNC : <推送服务>",
                   Sound = "YourSound"
               }
           };

            var idToCheck = new List<string>();
            var sendTask = client.SendPushMessageAsync(request);
            sendTask.ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("IsFaulted : " + task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Console.WriteLine("IsCanceled");
                }
                else
                {
                    Console.WriteLine("SendPushMessageAsync : " + task.Result.ResponseCode + ":" + task.Result.ResponseMessage);
                    Console.WriteLine("Push sent : " + task.Result.MessageId);

                    idToCheck.Add(task.Result.MessageId);


                }
            })
            .Wait(TimeSpan.FromSeconds(3))
            ;

            var statusList = client.QueryPushMessageStatus(idToCheck);
            Console.WriteLine("QueryPushMessageStatus is completed.");

            if (statusList != null)
            {
                foreach (var status in statusList)
                {
                    Console.WriteLine("Id: {0}, Android: {1}, iOS: {2}", status.MessageId, status.AndroidDeliveredCount,
                        status.ApplePushNotificationDeliveredCount);
                }
            }
        }

        private static void SendBroadcastMessage(JPushClient client)
        {
            var customizedValues = new Dictionary<string, string>();
            customizedValues.Add("CK1", "CV1");
            customizedValues.Add("CK2", "CV2");

            var request = new PushMessageRequest
            {
                MessageType = MessageType.Notification,
                Platform = PushPlatform.Android,
                Description = "DotNET",
                PushType = PushType.Broadcast,
                IsTestEnvironment = true,
                Message = new PushMessage
                {
                    Content = "JPush message @ " + DateTime.Now.ToLongTimeString(),
                    PushTitle = "<推送服务>",
                    Sound = "YourSound",
                    CustomizedValue = customizedValues
                }
            };

            var idToCheck = new List<string>();
            var response = client.SendPushMessage(request);

            Console.WriteLine("SendPushMessage : " + response.ResponseCode + ":" + response.ResponseMessage);
            Console.WriteLine("Push sent : " + response.MessageId);

            request.Message.PushTitle += "ASYNC";
            var sendTask = client.SendPushMessageAsync(request);
            sendTask.ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("IsFaulted : " + task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Console.WriteLine("IsCanceled");
                }
                else
                {
                    Console.WriteLine("SendPushMessageAsync : " + task.Result.ResponseCode + ":" + task.Result.ResponseMessage);
                    Console.WriteLine("Push sent : " + task.Result.MessageId);

                    idToCheck.Add(task.Result.MessageId);
                }
            })
            .Wait(TimeSpan.FromSeconds(3))
            ;

            idToCheck.Add(response.MessageId);

            var statusList = client.QueryPushMessageStatus(idToCheck);
            Console.WriteLine("QueryPushMessageStatus is completed.");

            if (statusList != null)
            {
                foreach (var status in statusList)
                {
                    Console.WriteLine("Id: {0}, Android: {1}, iOS: {2}", status.MessageId, status.AndroidDeliveredCount,
                        status.ApplePushNotificationDeliveredCount);
                }
            }
        }

        private static void ReadIniFile()
        {
            bool iniFound = false;

            // Read appKey and masterSecret from Settings.ini file
            // Example file:
            //     appkey=1234567890abcdef12345678
            //     mastersecret=abcdef1234567890aaaaaaaa
            foreach (var iniFile in new[] { "Settings.ini", "../../Settings.ini" })
            {
                try
                {
                    var iniSettings = File.ReadLines(iniFile).Select(l => l.Split('=')).ToDictionary(l => l.First().Trim(), l => l.Last().Trim());
                    appKey = iniSettings["appkey"]; // Your App Key from JPush
                    masterSecret = iniSettings["mastersecret"]; // Your Master Secret from JPush

                    iniFound = true;
                    break;
                }
                catch
                {
                    // Try other ini file
                }
            }

            if (!iniFound)
            {
                throw new Exception("Settings.ini not found or invalid");
            }
        }
    }
}