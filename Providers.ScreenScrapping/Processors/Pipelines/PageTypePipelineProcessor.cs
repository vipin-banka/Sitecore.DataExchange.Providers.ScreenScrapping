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
    public class PageTypePipelineProcessor : PipelineProcessor
    {
        public override bool CanProcess(Pipeline pipeline, PipelineContext pipelineContext)
        {
            if (base.CanProcess(pipeline, pipelineContext))
            {
                if (pipeline.HasPageTypeSettings() && pipelineContext.HasItemMetadataSettings())
                {
                    var pageTypeSettings = pipeline.GetPageTypeSettings();
                    var metadataSettings = pipelineContext.GetItemMetadataSettings();

                    if (!string.IsNullOrEmpty(pageTypeSettings.PageType)
                        &&
                        pageTypeSettings.PageType.Equals(metadataSettings.ItemMetadata.PageType,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
