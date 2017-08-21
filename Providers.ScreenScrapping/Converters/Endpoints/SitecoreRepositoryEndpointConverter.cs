using System;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.Endpoints
{
    public class SitecoreRepositoryEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.Endpoints.SitecoreRepositoryEndpointTemplateId);

        public SitecoreRepositoryEndpointConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //create the plugin
            var settings = new SitecoreRepositorySettings();
           
            settings.Repository = CustomContext.Repository;

            //add the plugin to the endpoint
            endpoint.Plugins.Add(settings);
        }
    }
}