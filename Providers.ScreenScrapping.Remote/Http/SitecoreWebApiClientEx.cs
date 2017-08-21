using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Sitecore.DataExchange.Remote.Http;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Remote.Http
{
    public class SitecoreWebApiClientEx : IDisposable
    {
        private HttpClient _client;
        private string _host;
        private string _domain;
        private string _userName;
        private string _password;
        private int _loginTimeout;
        private bool disposed;

        protected HttpClient HttpClient
        {
            get
            {
                if (this._client == null)
                {
                    this._client = new HttpClient();
                    this._client.BaseAddress = new Uri(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "https://{0}", new object[1]
                    {
            (object) this._host
                    }));
                }
                return this._client;
            }
        }

        protected DateTime LastLogin { get; set; }

        public SitecoreWebApiClientEx(ConnectionSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (string.IsNullOrWhiteSpace(settings.UserName))
                throw new ArgumentException(string.Format("No username is specified."), "settings");
            string[] strArray = settings.UserName.Split('\\');
            if (strArray.Length != 2)
                throw new ArgumentException(string.Format("Specified username is not in the required domain\\username format. (username: {0})", (object)settings.UserName), "settings");
            this._domain = strArray[0];
            if (string.IsNullOrWhiteSpace(this._domain))
                throw new ArgumentException(string.Format("No domain is specified on the username. (username: {0})", (object)settings.UserName), "settings");
            this._userName = strArray[1];
            if (string.IsNullOrWhiteSpace(this._userName))
                throw new ArgumentException(string.Format("No user is specified on the username. (username: {0})", (object)settings.UserName), "settings");
            this._password = settings.Password;
            if (string.IsNullOrWhiteSpace(this._password))
                throw new ArgumentException(string.Format("No password is specified."), "settings");
            this._host = settings.Host;
            if (string.IsNullOrWhiteSpace(this._host))
                throw new ArgumentException(string.Format("No host is specified."), "settings");
            this._loginTimeout = settings.LoginTimeout;
            ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback)((sender, cert, chain, sslPolicyErrors) => true);
        }

        public HttpResponseMessage DoPost<T>(string uri, IDictionary<string, string> parameters, T content)
        {
            if (!this.DoLoginIfNeeded())
                return (HttpResponseMessage)null;
            return this.HttpClient.PostAsync(this.Combine(uri, parameters), ConvertToStringContent(content)).Result;
        }

        public HttpResponseMessage DoPostContent(string uri, IDictionary<string, string> parameters, HttpContent content)
        {
            if (!this.DoLoginIfNeeded())
                return (HttpResponseMessage)null;
            return this.HttpClient.PostAsync(this.Combine(uri, parameters), content).Result;
        }

        protected string Combine(string uri, IDictionary<string, string> parameters)
        {
            string str1 = uri;
            if (parameters != null && parameters.Any<KeyValuePair<string, string>>())
            {
                string str2 = string.Join("&", parameters.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(p => string.Format("{0}={1}", (object)HttpUtility.UrlEncode(p.Key), (object)HttpUtility.UrlEncode(p.Value)))).ToArray<string>());
                str1 = string.Format("{0}?{1}", (object)uri, (object)str2);
            }
            return str1;
        }

        private bool DoLoginIfNeeded()
        {
            double num = Math.Round((DateTime.Now - this.LastLogin).TotalMinutes);
            if (num < (double)this._loginTimeout)
                return true;
            Context.Logger.Info("Last login {0} minutes ago. Login timeout is {1} minutes. Will login again.", (object)num, (object)this._loginTimeout);
            if (!this.HttpClient.PostAsync("sitecore/api/ssc/auth/login", (HttpContent)this.ConvertToStringContent((object)new Dictionary<string, object>()
      {
        {
          "domain",
          (object) this._domain
        },
        {
          "username",
          (object) this._userName
        },
        {
          "password",
          (object) this._password
        }
      })).Result.StatusCode.Equals((object)HttpStatusCode.OK))
            {
                Context.Logger.Error("Login failed.");
                return false;
            }
            this.LastLogin = DateTime.Now;
            Context.Logger.Info("Login success.");
            return true;
        }

        private StringContent ConvertToStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.disposed)
                return;
            this.disposed = true;
            this.HttpClient.Dispose();
        }
    }
}
