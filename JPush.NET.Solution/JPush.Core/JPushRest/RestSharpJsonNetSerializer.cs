using Newtonsoft.Json;
using RestSharp.Serializers;

namespace JPush.Core.JPushRest
{
    public class RestSharpJsonNetSerializer : ISerializer
    {
        public string ContentType { get; set; }
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public RestSharpJsonNetSerializer()
        {
            ContentType = "application/json";
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}