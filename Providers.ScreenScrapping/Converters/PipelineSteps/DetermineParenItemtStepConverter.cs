using System;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.PipelineSteps
{
    public class DetermineParenItemtStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.PipelineSteps.DetermineParentItemPipelineStepTemplateId);

        public DetermineParenItemtStepConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }


        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new ParentItemSettings();
            settings.ParentItemId = base.GetStringValue(source, "ParentItem");
            pipelineStep.Plugins.Add(settings);
        }
    }
}