using System;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Converters.Pipelines;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.Pipelines
{
    public class PageTypePipelineConverter : PipelineConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.Pipelines.PageTypePipelineTemplateId);
        public PageTypePipelineConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, Pipeline pipeline)
        {
            //create the plugin
            var settings = new PageTypeSettings();

            //populate the plugin using values from the item
            settings.PageType =
                base.GetStringValue(source, Constants.Fields.PageType);

            //add the plugin to the endpoint
            pipeline.Plugins.Add(settings);
        }
    }
}