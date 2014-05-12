using System;
using System.Collections.Generic;
using JPush;
using JPush.Models;

namespace JPushConsoleExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var appKey = "appkey"; // Your App Key from JPush
            var masterSecret = "mastersecret"; // Your Master Secret from JPush

            var customizedValues = new Dictionary<string, string>();
            customizedValues.Add("CK1", "CV1");
            customizedValues.Add("CK2", "CV2");

            var client = new JPushClient(appKey, masterSecret, false);
            //client.OverrideApiUrl("http://localhost", 10000);
            //client.OverrideReportUrl("http://localhost", 10001);

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

                    idToCheck.Add(response.MessageId);
                }
            });

            sendTask.Wait(TimeSpan.FromSeconds(1));

            idToCheck.Add(response.MessageId);

            var statusList = client.QueryPushMessageStatus(idToCheck);
            Console.WriteLine("QueryPushMessageStatus is completed.");

            if (statusList != null)
            {
                foreach (var status in statusList)
                {
                    Console.WriteLine("Id: {0}, Android: {1}, iOS: {2}", status.MessageId, status.AndroidDeliveredCount, status.ApplePushNotificationDeliveredCount);
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}