using RestSharp;

namespace JPush.JPushRest
{
	public class JPushRestClient : RestClient
	{
		public JPushRestClient(string baseUrl, string appKey, string masterKey) : base(baseUrl)
		{
			AddHandler("text/json", new RestSharpDataContractJsonDeserializer());
			AddHandler("application/json", new RestSharpDataContractJsonDeserializer());
			AddHandler("text/html", new RestSharpDataContractJsonDeserializer());

			Authenticator = new HttpBasicAuthenticator(appKey, masterKey);
		}
	}
}