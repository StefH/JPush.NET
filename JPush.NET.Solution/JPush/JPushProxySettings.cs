using System;
using System.Net;

namespace JPush
{
	public class JPushProxySettings
	{
		public bool Enabled { get; private set; }
		public string Url { get; private set; }
		public string User { get; private set; }
		public string Password { get; private set; }
		public string Domain { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="JPushProxySettings"/> class.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public JPushProxySettings(bool enabled)
		{
			Enabled = enabled;
			Url = string.Empty;
			User = string.Empty;
			Password = string.Empty;
			Domain = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JPushProxySettings"/> class.
		/// </summary>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		/// <param name="url">The URL.</param>
		/// <param name="user">The user.</param>
		/// <param name="password">The password.</param>
		/// <param name="domain">The domain.</param>
		public JPushProxySettings(bool enabled, string url, string user, string password, string domain)
		{
			Enabled = enabled;
			Url = url;
			User = user;
			Password = password;
			Domain = domain;
		}

		/// <summary>
		/// Gets the proxy settings as IWebProxy object.
		/// </summary>
		/// <returns></returns>
		public IWebProxy GetWebProxy()
		{
			if (string.IsNullOrEmpty(Url))
			{
				return WebRequest.DefaultWebProxy;
			}

			var proxy = new WebProxy
			{
				// Set URL
				Address = new Uri(Url)
			};

			// Set username and password if available, else use the default credentials.
			if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password))
			{
				proxy.Credentials = string.IsNullOrEmpty(Domain)
										? new NetworkCredential(User, Password)
										: new NetworkCredential(User, Password, Domain);
			}
			else
			{
				proxy.UseDefaultCredentials = true;
			}

			return proxy;
		}
	}
}