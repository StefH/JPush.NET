﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JPush.Core;
using JPush.Core.ExternalModels;
using JPush.V2;
using JPush.V2.ExternalModels;

namespace JPushConsoleExampleV2
{
    class Program
    {
        private static string _appKey; // Your App Key from JPush
        private static string _masterSecret; // Your Master Secret from JPush
        private static string _proxyUrl;

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            ReadIniFile();

            try
            {
                // Can fail
                SendByRegistrationIdMessage(true);
            }
            finally
            {
                SendByRegistrationIdMessage();
                SendBroadcastMessage();

                Console.WriteLine("Press any key to exit.");
                Console.Read();
            }
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Error : {0}", e.ExceptionObject);
        }

        private static void SendByRegistrationIdMessage(bool useLocalhost = false)
        {
            var proxy = !string.IsNullOrEmpty(_proxyUrl) ? new JPushProxySettings(true, _proxyUrl, "", "", "") : null;

            var client = new JPushClientV2(_appKey, _masterSecret, false, proxy);
            if (useLocalhost)
            {
                Console.WriteLine("Call http://localhost:10000/v2");
                client.OverrideApiUrl("http://localhost", 10000);
                client.OverrideReportUrl("http://localhost", 10000);
            }

            var request = new PushMessageRequest
            {
                MessageType = MessageType.Notification,
                Platform = PushPlatform.Android,
                Description = "DotNET",
                PushType = PushType.ByRegistrationId,
                ReceiverValue = "0606a164ac1",
                IsTestEnvironment = true,
                Message = new PushMessage
                {
                    Content = "JPush ByRegistrationId message @ " + DateTime.Now.ToLongTimeString(),
                    PushTitle = "<推送服务>",
                    Sound = "YourSound"
                }
            };

            var response = client.SendPushMessage(request);

            if (response.ResponseCode == PushResponseCode.Succeed)
            {
                var idToCheck = new List<string> { response.MessageId };

                var statusList = client.QueryPushMessageStatus(idToCheck);
                Console.WriteLine("QueryPushMessageStatus is completed.");

                if (statusList != null)
                {
                    foreach (var status in statusList)
                    {
                        Console.WriteLine("Id: {0}, Android: {1}, iOS: {2}",
                            status.MessageId, status.AndroidDeliveredCount ?? 0, status.ApplePushNotificationDeliveredCount ?? 0);
                    }
                }
            }
            else
            {
                Console.WriteLine("There was an error [{0}, {1}] when sending a PushMessage", response.ResponseCode, response.ResponseMessage);
            }
        }

        private static void SendBroadcastMessage()
        {
            var client = new JPushClientV2(_appKey, _masterSecret);

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

            if (response.ResponseCode == PushResponseCode.Succeed)
            {

                request.Message.PushTitle += "ASYNC";
                var sendTask = client.SendPushMessageAsync(request);
                sendTask.ContinueWith(task =>
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
                        Console.WriteLine("SendPushMessageAsync : " + task.Result.ResponseCode + ":" +
                                          task.Result.ResponseMessage);
                        Console.WriteLine("Push sent : " + task.Result.MessageId);

                        idToCheck.Add(task.Result.MessageId);
                    }
                })
                    .Wait(TimeSpan.FromSeconds(10))
                    ;

                idToCheck.Add(response.MessageId);

                var statusList = client.QueryPushMessageStatus(idToCheck);
                Console.WriteLine("QueryPushMessageStatus is completed.");

                if (statusList != null)
                {
                    foreach (var status in statusList)
                    {
                        Console.WriteLine("Id: {0}, Android: {1}, iOS: {2}",
                            status.MessageId, status.AndroidDeliveredCount ?? 0,
                            status.ApplePushNotificationDeliveredCount ?? 0);
                    }
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
            //     proxyurl=http://localhost:8888
            foreach (var iniFile in new[] { "Settings.ini", "../../Settings.ini" })
            {
                try
                {
                    var iniSettings = File.ReadLines(iniFile).Select(l => l.Split('=')).ToDictionary(l => l.First().Trim(), l => l.Last().Trim());
                    _appKey = iniSettings["appkey"]; // Your App Key from JPush
                    _masterSecret = iniSettings["mastersecret"]; // Your Master Secret from JPush
                    _proxyUrl = iniSettings.ContainsKey("proxyurl") ? iniSettings["proxyurl"] : null;

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