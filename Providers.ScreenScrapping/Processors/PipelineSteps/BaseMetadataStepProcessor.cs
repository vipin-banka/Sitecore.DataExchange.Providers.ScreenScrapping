using System;
using System.Collections.Generic;
using System.Data;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    public abstract class BaseMetadataStepProcessor : BaseReadDataStepProcessor
    {
        protected ILogger Logger;

        private IList<ItemMetadata> _metadataItems;

        private void AddMetadataSettingsPluginToPipelineContext(PipelineContext pipelineContext)
        {
            if (!pipelineContext.HasIterableDataSettings())
            {
                pipelineContext.Plugins.Add(new IterableDataSettings(new List<ItemMetadata>()));
            }

            _metadataItems = pipelineContext.GetIterableDataSettings().Data as IList<ItemMetadata>;
        }

        protected override void ReadData(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {

            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException(nameof(pipelineStep));
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException(nameof(pipelineContext));
            }
            Logger = pipelineContext.PipelineBatchContext.Logger;

            AddMetadataSettingsPluginToPipelineContext(pipelineContext);
            ReadMetadata(endpoint, pipelineStep, pipelineContext);
        }

        protected abstract void ReadMetadata(Endpoint endpoint, PipelineStep pipelineStep, PipelineContext pipelineContext);

        protected void AddMetadata(Func<ItemMetadata, bool> action)
        {
            this.AddMetadata(null, action);
        }


        protected void AddMetadata(ItemMetadata itemMetadata, Func<ItemMetadata, bool> action)
        {
            if (itemMetadata == null)
            {
                itemMetadata = new ItemMetadata();
            }

            if (action(itemMetadata))
            {
                _metadataItems.Add(itemMetadata);
            }
        }
    }
}