using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RestSharp.Serializers;

namespace JPush.JPushRest
{
    public class RestSharpDataContractJsonSerializer : ISerializer
    {
        public string ContentType { get; set; }
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public string Serialize(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);

                var json = memoryStream.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }
    }
}