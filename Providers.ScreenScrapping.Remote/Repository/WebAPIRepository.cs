using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Remote.Http;
using Sitecore.DataExchange.Providers.ScreenScrapping.Repositories;
using Sitecore.DataExchange.Remote.Http;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Sitecore.DataExchange.Providers.ScreenScrapping.Remote.Extensions;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Remote.Repository
{
    public class WebApiContentRepository : IContentRepository, IDisposable
    {
        private readonly ConnectionSettings _settings;
        private SitecoreWebApiClientEx _client;
        private bool _disposed;

        private string relativeUri =
            "sitecore/api/ssc/Sitecore-DataExchange-Providers-ScreenScrapping-Sc/SitecoreItem/-/";
        public string DatabaseName { get; private set; }

        protected SitecoreWebApiClientEx Client
        {
            get
            {
                if (this._client == null)
                    this._client = new SitecoreWebApiClientEx(this._settings);
                return this._client;
            }
        }

        public WebApiContentRepository(string databaseName, ConnectionSettings settings)
        {
            if (databaseName == null)
                throw new ArgumentNullException("databaseName");
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException(string.Format("No value is specified."), "databaseName");
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.DatabaseName = databaseName;
            this._settings = settings;
        }

        public void Save(CreateUpdateItemInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            string uri = relativeUri + "CreateOrUpdate";
            HttpResponseMessage httpResponseMessage = this.Client.DoPost(uri, null, info);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                Context.Logger.Error("{0} method {1} failed. (status code: {2}, id: {3})",
                    (object) this.GetType().FullName, (object)"CreateOrUpdate", (object) httpResponseMessage.StatusCode);
            }
            else
            {
                var itemInfo = JsonConvert.DeserializeObject<CreateUpdateItemInfo>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                itemInfo.Fields = info.Fields;
                info = itemInfo;
            }
        }

        public IList<string> ResolveFieldValue(FieldValueSearchOptions fieldValueSearchOptions)
        {
            if (fieldValueSearchOptions == null)
                throw new ArgumentNullException("fieldValueSearchOptions");
            string uri = relativeUri + "ResolveFieldValue";
            HttpResponseMessage httpResponseMessage = this.Client.DoPost(uri, null, fieldValueSearchOptions);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                Context.Logger.Error("{0} method {1} failed. (status code: {2}",
                    (object)this.GetType().FullName, (object)"ResolveFieldValue", (object)httpResponseMessage.StatusCode);
            }
            else
            {
                return JsonConvert.DeserializeObject<IList<string>>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }

            return null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this._disposed)
                return;
            this._disposed = true;
            this.Client.Dispose();
        }

        public void SaveMedia(CreateUpdateMediaInfo info, string fileName, Stream stream)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (stream == null)
                throw new ArgumentNullException("stream");
            string uri = relativeUri + "UploadMedia";
            using (var content = new MultipartFormDataContent())
            {
                var optionsContent = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8,
                    "application/json");
                optionsContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("options")
                {
                    Name = "options",
                    FileName = ""
                };

                var fileContent =
                    new ByteArrayContent(stream.ReadAllBytes());
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

                content.Add(fileContent);
                content.Add(optionsContent);


                HttpResponseMessage httpResponseMessage = this.Client.DoPostContent(uri, null, content);
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    Context.Logger.Error("{0} method {1} failed. (status code: {2}",
                        (object) this.GetType().FullName, (object) "UploadMedia",
                        (object) httpResponseMessage.StatusCode);
                }
                else
                {
                    var result =
                        JsonConvert.DeserializeObject<IDictionary<string,string>>(
                            httpResponseMessage.Content.ReadAsStringAsync().Result);
                    if (result != null && result.Any() && result.ContainsKey(fileName))
                    {
                        info.MediaId = result[fileName];
                    }
                }
            }
        }
    }
}