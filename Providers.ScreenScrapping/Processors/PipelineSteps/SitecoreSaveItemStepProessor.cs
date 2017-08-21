using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    public class SitecoreSaveItemStepProcessor : BasePipelineStepProcessor
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
                var endpointSettings = pipelineStep.GetEndpointSettings();
                var sitecoreRepositorySettings = endpointSettings.EndpointFrom.GetSitecoreRepositorySettings();
                var itemMetadataSettings = pipelineContext.GetItemMetadataSettings();
                var createUpdateInfo = Map(itemMetadataSettings.ItemDetail);
                sitecoreRepositorySettings.Repository.Save(createUpdateInfo);
            }
        }

        private CreateUpdateItemInfo Map(ItemDetail itemdetail)
        {
            var createUpdateInfo = new CreateUpdateItemInfo();
            createUpdateInfo.Metadata.LanguageName = itemdetail.LanguageName;
            createUpdateInfo.Metadata.DatabaseName = itemdetail.DatatabaseName;
            createUpdateInfo.Metadata.ItemName = itemdetail.ItemName;
            createUpdateInfo.Metadata.ParentItemId = itemdetail.ParentItemId;
            createUpdateInfo.Metadata.TemplateId = itemdetail.TemplateId;
            createUpdateInfo.Fields = itemdetail.Fields;
            return createUpdateInfo;
        }
    }
}