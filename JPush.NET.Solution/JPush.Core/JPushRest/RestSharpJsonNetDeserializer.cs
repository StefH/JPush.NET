using System.Globalization;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace JPush.Core.JPushRest
{
    public class RestSharpJsonNetDeserializer : IDeserializer
    {
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public CultureInfo Culture { get; set; }

        public RestSharpJsonNetDeserializer()
        {
            Culture = CultureInfo.InvariantCulture;
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}