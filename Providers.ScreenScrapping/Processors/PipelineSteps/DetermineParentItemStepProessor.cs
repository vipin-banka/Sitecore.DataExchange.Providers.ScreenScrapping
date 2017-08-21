using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    //[RequiredEndpointPlugins(typeof(WebPageReaderSettings))]
    public class DetermineParentItemStepProessor : BasePipelineStepProcessor
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
                var parentItemSettings = pipelineStep.GetParentItemSettings();
                if (parentItemSettings == null || string.IsNullOrEmpty(parentItemSettings.ParentItemId))
                {
                    logger.Error(
                        "Pipeline step processing will abort because the pipeline step has no parent item assigned. (pipeline step: {0})",
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

                        itemMetadataSettings.ItemDetail.ParentItemId = parentItemSettings.ParentItemId;
                    }
                }
            }
        }
    }
}