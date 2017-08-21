using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.Pipelines;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.Pipelines
{
    public class VerifyDataReadPipelineProcessor : PipelineProcessor
    {
        public override bool CanProcess(Pipeline pipeline, PipelineContext pipelineContext)
        {
            if (base.CanProcess(pipeline, pipelineContext))
            {
                if (pipelineContext.HasItemMetadataSettings())
                {
                    var metadataSettings = pipelineContext.GetItemMetadataSettings();

                    if (metadataSettings.ItemDetail != null 
                        && metadataSettings.ItemDetail.Fields != null
                        && metadataSettings.ItemDetail.Fields.Any()
                        && !string.IsNullOrEmpty(metadataSettings.ItemDetail.ParentItemId)
                        && !string.IsNullOrEmpty(metadataSettings.ItemDetail.TemplateId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
