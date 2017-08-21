using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    //[RequiredEndpointPlugins(typeof(WebPageReaderSettings))]
    public class ResolveTagStepProcessor : BasePipelineStepProcessor
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
                var fieldValueResolverSettings = pipelineStep.GetFieldValueResolverSettings();
                if (fieldValueResolverSettings == null)
                {
                    logger.Error(
                        "Pipeline step processing will abort because the pipeline step has no field value resolver assigned. (pipeline step: {0})",
                        (object)pipelineStep.Name);
                }
                else
                {
                    var endpointSettings = pipelineStep.GetEndpointSettings();
                    if (endpointSettings == null)
                    {

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

                            if (itemMetadataSettings.ItemDetail.ResolveFields != null &&
                                itemMetadataSettings.ItemDetail.ResolveFields.Any())
                            {
                                var resolveField =
                                    itemMetadataSettings.ItemDetail.ResolveFields.FirstOrDefault(
                                        x =>
                                            x.Key.Equals(fieldValueResolverSettings.Key,
                                                StringComparison.OrdinalIgnoreCase));
                                if (resolveField.Value != null)
                                {
                                    var fieldValues = endpointSettings.EndpointFrom.GetSitecoreRepositorySettings()
                                        .Repository.ResolveFieldValue(Map(fieldValueResolverSettings,
                                            itemMetadataSettings.ItemMetadata, resolveField.Value as IList<string>));
                                    if (fieldValues != null && fieldValues.Any())
                                    {
                                        itemMetadataSettings.ItemDetail.Fields[resolveField.Key] = String.Join("|",
                                            fieldValues);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private FieldValueSearchOptions Map(FieldValueResolverSettings fieldValueResolverSettings, ItemMetadata itemMetadata, IList<string> values)
        {
            var result = fieldValueResolverSettings as FieldValueSearchOptions;
            result.LanguageName = itemMetadata.LanguageName;
            result.Values = values;
            return result;
        }
    }
}