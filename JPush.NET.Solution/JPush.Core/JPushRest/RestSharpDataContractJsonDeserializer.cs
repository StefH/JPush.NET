using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;

namespace JPush.Core.JPushRest
{
    public class RestSharpDataContractJsonDeserializer : IDeserializer
    {
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
        public CultureInfo Culture { get; set; }

        public RestSharpDataContractJsonDeserializer()
        {
            Culture = CultureInfo.InvariantCulture;
        }

        public T Deserialize<T>(IRestResponse response)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}