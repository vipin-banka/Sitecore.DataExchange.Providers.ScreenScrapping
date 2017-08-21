using System;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.Endpoints
{
    public class ExcelFileEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.Endpoints.ExcelFileEndpointTemplateId);
        public ExcelFileEndpointConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //create the plugin
            var settings = new ExcelFileSettings();
           
            //populate the plugin using values from the item
            settings.SheetName =
                base.GetStringValue(source, Constants.Fields.SheetName);
            settings.Path =
                base.GetStringValue(source, Constants.Fields.Path);

            //add the plugin to the endpoint
            endpoint.Plugins.Add(settings);
        }
    }
}