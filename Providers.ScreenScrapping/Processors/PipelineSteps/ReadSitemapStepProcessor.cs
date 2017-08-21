using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    //[RequiredEndpointPlugins(typeof(SitemapSettings))]
    public class ReadSitemapStepProcessor : BaseMetadataStepProcessor
    {
        protected override void ReadMetadata(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            //get the file path from the plugin on the endpoint
            var settings = endpoint.GetSitemapSettings();
            if (settings == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(settings.Url))
            {
                Logger.Error(
                    "No url is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }

            //read the url
            var urls = GetUrls(settings.Url).ToList();

            DataRowCollection rows = null;

            if (urls.Any())
            {
                var firstNews = urls.FirstOrDefault(i => i.ToLower().Contains("/news/"));
                var firstEvents = urls.FirstOrDefault(i => i.ToLower().Contains("/events/"));
                urls = new List<string> {firstEvents, firstNews};

                foreach (var urlItem in urls)
                {
                    this.AddMetadata(itemMetadata => GetItemMetadata(urlItem, itemMetadata));
                }
            }

            Logger.Info("{0} rows were read from sitemap. (pipeline step: {1}, endpoint: {2})",
                urls.Count, pipelineStep.Name, endpoint.Name);
        }

        private bool GetItemMetadata(string url, ItemMetadata itemMetadata)
        {
            itemMetadata.Url = url;
            return true;
        }

        private IEnumerable<string> GetUrls(string url)
        {
            List<string> urls = new List<string>();
            XmlReader xmlReader = new XmlTextReader(url);
            XPathDocument document = new XPathDocument(xmlReader);
            XPathNavigator navigator = document.CreateNavigator();

            XmlNamespaceManager resolver = new XmlNamespaceManager(xmlReader.NameTable);
            resolver.AddNamespace("sitemap", "http://www.sitemaps.org/schemas/sitemap/0.9");

            XPathNodeIterator iterator = navigator.Select("/sitemap:urlset/sitemap:url/sitemap:loc", resolver);

            while (iterator.MoveNext())
            {
                if (iterator.Current == null)
                    continue;

                urls.Add(iterator.Current.Value);
            }

            return urls;
        }
    }
}