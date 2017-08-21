using HtmlAgilityPack;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    //[RequiredEndpointPlugins(typeof(WebPageReaderSettings))]
    public abstract class BasePageReadStepProessor : BasePipelineStepProcessor
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
                if (!pipelineContext.HasItemMetadataSettings())
                {
                    logger.Error(
                        "Pipeline step processing will abort because the required item metadata not found. (pipeline step: {0})",
                        (object)pipelineStep.Name);
                }
                else
                {
                    var itemMetadataSettings = pipelineContext.GetItemMetadataSettings();

                    itemMetadataSettings.ItemDetail.LanguageName = itemMetadataSettings.ItemMetadata.LanguageName;

                    if (!string.IsNullOrEmpty(itemMetadataSettings.ItemMetadata.ItemName))
                    {
                        itemMetadataSettings.ItemDetail.ItemName = itemMetadataSettings.ItemMetadata.ItemName;
                    }
                    else
                    {
                        itemMetadataSettings.ItemDetail.ItemName =
                            itemMetadataSettings.ItemMetadata.Url.GetLastPart();
                    }

                    HtmlWeb web = new HtmlWeb();
                    HtmlDocument document = web.Load(itemMetadataSettings.ItemMetadata.Url);

                    ReadContent(document, itemMetadataSettings.ItemMetadata,
                        itemMetadataSettings.ItemDetail);
                }
            }
        }

        protected abstract void ReadContent(HtmlDocument document, ItemMetadata metadata, ItemDetail itemDetail);
    }
}