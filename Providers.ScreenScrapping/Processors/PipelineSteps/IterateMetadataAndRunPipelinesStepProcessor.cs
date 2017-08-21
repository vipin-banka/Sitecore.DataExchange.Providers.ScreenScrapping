using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    public class IterateMetadataAndRunPipelinesStepProcessor : Sitecore.DataExchange.Processors.PipelineSteps.BasePipelineStepProcessor
    {
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            ILogger logger = pipelineContext.PipelineBatchContext.Logger;
            if (!this.CanProcess(pipelineStep, pipelineContext))
            {
                logger.Error("Pipeline step processing will abort because the pipeline step cannot be processed. (pipeline step: {0})", (object)pipelineStep.Name);
            }
            else
            {
                PipelinesSettings pipelinesSettings = pipelineStep.GetPipelinesSettings();
                if (pipelinesSettings == null || !pipelinesSettings.Pipelines.Any<Sitecore.DataExchange.Models.Pipeline>())
                {
                    logger.Error("Pipeline step processing will abort because the pipeline step has no sub-pipelines assigned. (pipeline step: {0})", (object)pipelineStep.Name);
                }
                else
                {
                    var iterableMetaDataSettings = pipelineContext.GetIterableDataSettings();
                    if (iterableMetaDataSettings == null || iterableMetaDataSettings.Data == null)
                        return;
                    int num = 0;
                    try
                    {
                        foreach (var element in iterableMetaDataSettings.Data)
                        {
                            if (!pipelineContext.PipelineBatchContext.Stopped)
                            {
                                PipelineContext pipelineContext1 = new PipelineContext(pipelineContext.PipelineBatchContext);
                                var itemMetadataSetting = new ItemMetadataSettings(element as ItemMetadata);
                                pipelineContext1.Plugins.Add(itemMetadataSetting);
                                ParentPipelineContextSettings pipelineContextSettings = new ParentPipelineContextSettings()
                                {
                                    ParentPipelineContext = pipelineContext
                                };
                                pipelineContext1.Plugins.Add(pipelineContextSettings);
                                this.ProcessPipelines(pipelineStep, pipelinesSettings.Pipelines, pipelineContext1);
                                ++num;
                            }
                            else
                                break;
                        }
                        logger.Info("{0} elements were iterated. (pipeline: {1}, pipeline step: {2})", (object)num, (object)pipelineContext.CurrentPipeline.Name, (object)pipelineContext.CurrentPipelineStep.Name, (object)pipelineContext);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Error(ex.StackTrace);
                        pipelineContext.CriticalError = true;
                    }
                }
            }
        }

        protected virtual void ProcessPipelines(PipelineStep pipelineStep, ICollection<Sitecore.DataExchange.Models.Pipeline> subPipelines, PipelineContext pipelineContext)
        {
            if (pipelineStep == null)
                throw new ArgumentNullException("pipelineStep");
            if (subPipelines == null)
                throw new ArgumentNullException("subPipelines");
            if (pipelineContext == null)
                throw new ArgumentNullException("pipelineContext");
            ILogger logger = pipelineContext.PipelineBatchContext.Logger;
            if (!subPipelines.Any<Sitecore.DataExchange.Models.Pipeline>())
            {
                logger.Error("Pipeline step processing will abort because no pipelines are assigned to the pipeline step. (pipeline step: {0})", (object)pipelineStep.Name);
            }
            else
            {
                List<Sitecore.DataExchange.Models.Pipeline> pipelineList = new List<Sitecore.DataExchange.Models.Pipeline>();
                foreach (Sitecore.DataExchange.Models.Pipeline subPipeline in (IEnumerable<Sitecore.DataExchange.Models.Pipeline>)subPipelines)
                {
                    pipelineContext.CurrentPipeline = subPipeline;
                    IPipelineProcessor pipelineProcessor = subPipeline.PipelineProcessor;
                    if (pipelineProcessor == null)
                        logger.Error("Pipeline will be skipped because it does not have a processor assigned. (pipeline step: {0}, sub-pipeline: {1})", (object)pipelineStep.Name, (object)subPipeline.Name);
                    else if (!pipelineProcessor.CanProcess(subPipeline, pipelineContext))
                    {
                        logger.Error("Pipeline will be skipped because the processor cannot processes the sub-pipeline. (pipeline step: {0}, sub-pipeline: {1}, sub-pipeline processor: {2})", (object)pipelineStep.Name, (object)subPipeline.Name, (object)pipelineProcessor.GetType().FullName);
                    }
                    else
                    {
                        pipelineProcessor.Process(subPipeline, pipelineContext);
                        if (pipelineContext.CriticalError)
                        {
                            logger.Error("Sub pipeline processing will abort because a critical error occurred during processing. (pipeline step: {0}, sub-pipeline: {1}, sub-pipeline processor: {2})", (object)pipelineStep.Name, (object)subPipeline.Name, (object)pipelineProcessor.GetType().FullName);
                            this.OnCriticalError(subPipeline, (IEnumerable<Sitecore.DataExchange.Models.Pipeline>)pipelineList, pipelineStep, pipelineContext);
                            break;
                        }
                    }
                }
            }
        }

        protected virtual void OnCriticalError(Sitecore.DataExchange.Models.Pipeline errorPipeline, IEnumerable<Sitecore.DataExchange.Models.Pipeline> completedPipelines, PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
        }
    }
}