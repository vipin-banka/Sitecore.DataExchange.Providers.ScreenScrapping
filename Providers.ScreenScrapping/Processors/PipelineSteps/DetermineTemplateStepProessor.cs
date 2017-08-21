using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    //[RequiredEndpointPlugins(typeof(WebPageReaderSettings))]
    public class DetermineTemplateStepProessor : BasePipelineStepProcessor
    {
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            ILogger logger = pipelineContext.PipelineBatchContext.Logger;
            if (!this.CanProcess(pipelineStep, pipelineContext))
            {
                logger.Error(
                    "Pipeline step processing will abort because the pipeline step cannot be processed. (pipeline step: {0})",
                    (object)pipelineStep.Name);
            }
            else
            {
                var templateSettings = pipelineStep.GetTemplateSettings();
                if (templateSettings == null || string.IsNullOrEmpty(templateSettings.TemplateId))
                {
                    logger.Error(
                        "Pipeline step processing will abort because the pipeline step has no template assigned. (pipeline step: {0})",
                        (object)pipelineStep.Name);
                }
                else
                {
                    if (!pipelineContext.HasItemMetadataSettings())
                    {
                        logger.Error(
                    "Pipeline step processing will abort because the required item metadata not found. (pipeline step: {0})",
                    (object)pipelineStep.Name);
                    }
                    else
                    {
                        var itemMetadataSettings = pipelineContext.GetItemMetadataSettings();

                        itemMetadataSettings.ItemDetail.TemplateId = templateSettings.TemplateId;
                    }
                }
            }
        }
    }
}