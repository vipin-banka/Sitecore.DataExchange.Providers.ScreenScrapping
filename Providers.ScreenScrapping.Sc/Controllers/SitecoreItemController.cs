using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Http;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Controllers
{
    [ServicesController]
    public class SitecoreItemController : ServicesApiController
    {
        [HttpPost]
        public CreateUpdateItemInfo CreateOrUpdate(CreateUpdateItemInfo info)
        {
            CustomContext.Repository.Save(info);
            info.Fields = null;
            return info;
        }

        [HttpPost]
        public IList<string> ResolveFieldValue(FieldValueSearchOptions info)
        {
            var result = CustomContext.Repository.ResolveFieldValue(info);
            return result;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UploadMedia()
        {
            // Check whether the POST operation is MultiPart?
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Prepare CustomMultipartFormDataStreamProvider in which our multipart form
            // data will be loaded.
            string fileSaveLocation = HttpContext.Current.Server.MapPath("~/temp");
            if (!Directory.Exists(fileSaveLocation))
            {
                Directory.CreateDirectory(fileSaveLocation);
            }

            fileSaveLocation = Path.Combine(fileSaveLocation, "def-media");
            if (!Directory.Exists(fileSaveLocation))
            {
                Directory.CreateDirectory(fileSaveLocation);
            }

            fileSaveLocation = Path.Combine(fileSaveLocation, Guid.NewGuid().ToString());
            if (!Directory.Exists(fileSaveLocation))
            {
                Directory.CreateDirectory(fileSaveLocation);
            }

            CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            var files = new Dictionary<string, string>();

            try
            {
                // Read all contents of multipart message into CustomMultipartFormDataStreamProvider.
                await Request.Content.ReadAsMultipartAsync(provider);

                var options = JsonConvert.DeserializeObject<CreateUpdateMediaInfo>(provider.FormData["options"]);

                foreach (MultipartFileData file in provider.FileData)
                {
                    using (var fs = File.Open(file.LocalFileName, FileMode.Open))
                    {
                        var fileName = Path.GetFileName(file.LocalFileName);
                        CustomContext.Repository.SaveMedia(options, fileName, fs);
                        if (!string.IsNullOrEmpty(fileName) && !files.ContainsKey(fileName))
                        {
                            files.Add(fileName, options.MediaId);
                        }
                    }
                }

                Directory.Delete(fileSaveLocation, true);

                // Send OK Response along with saved file names to the client.
                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}