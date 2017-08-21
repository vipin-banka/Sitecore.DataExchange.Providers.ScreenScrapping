using System;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.Endpoints
{
    public class SitemapEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.Endpoints.SitemapEndpointTemplateId);
        public SitemapEndpointConverter(IItemModelRepository repository) : base(repository)
        {
            //identify the template an item must be based
            //on in order for the converter to be able to
            //convert the item
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //
            //create the plugin
            var settings = new SitemapSettings();
            //
            //populate the plugin using values from the item

            settings.Url =
                base.GetStringValue(source, Constants.Fields.Url);

            //add the plugin to the endpoint
            endpoint.Plugins.Add(settings);
        }
    }
}
