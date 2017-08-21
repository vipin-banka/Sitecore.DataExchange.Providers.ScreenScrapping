using System;
using System.Collections.Generic;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.PipelineSteps
{
    public class IterateMetadataAndRunPipelinesStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.PipelineSteps.IterateMeatadataPipelineStepTemplateId);
        public IterateMetadataAndRunPipelinesStepConverter(IItemModelRepository repository)
      : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            this.AddPipelinesSettingsPlugin(source, pipelineStep);
        }

        public override PipelineStep Convert(ItemModel source)
        {
            this.CanConvert(source);
            return base.Convert(source) ?? (PipelineStep)null;
        }

        private void AddPipelinesSettingsPlugin(ItemModel source, PipelineStep pipelineStep)
        {
            PipelinesSettings pipelinesSettings = new PipelinesSettings();
            IEnumerable<Sitecore.DataExchange.Models.Pipeline> models = this.ConvertReferencesToModels<Sitecore.DataExchange.Models.Pipeline>(source, "Pipelines");
            if (models != null)
            {
                foreach (var pipeline in models)
                    pipelinesSettings.Pipelines.Add(pipeline);
            }
            pipelineStep.Plugins.Add(pipelinesSettings);
        }
    }
}