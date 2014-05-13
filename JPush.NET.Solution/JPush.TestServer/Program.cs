using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace JPushTestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new JPushServices();
            var binding = new WebHttpBinding();

            var serviceHost = new WebServiceHost(services, new Uri("http://localhost:10000/v2/"));
            serviceHost.AddServiceEndpoint(typeof(JPushServices), binding, "");
            serviceHost.Open();

            Console.ReadKey();
            serviceHost.Close();
        }
    }
}
