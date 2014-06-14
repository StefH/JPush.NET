using RestSharp;
using RestSharp.Deserializers;

namespace JPush.Core.JPushRest
{
    public class JPushRestClient : RestClient
    {
        public JPushRestClient(string baseUrl, string appKey, string masterKey, IDeserializer deserializer = null)
            : base(baseUrl)
        {
            Authenticator = new HttpBasicAuthenticator(appKey, masterKey);

            RemoveHandlers();
            AddHandlers(deserializer ?? new RestSharpDataContractJsonDeserializer());
        }

        private void RemoveHandlers()
        {
            RemoveHandler("text/json");
            RemoveHandler("application/json");
            RemoveHandler("text/html");
        }

        private void AddHandlers(IDeserializer deserializer)
        {
            AddHandler("text/json", deserializer);
            AddHandler("application/json", deserializer);
            AddHandler("text/html", deserializer);
        }
    }
}