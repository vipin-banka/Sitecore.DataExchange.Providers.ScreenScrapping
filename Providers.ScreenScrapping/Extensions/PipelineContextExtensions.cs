using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Extensions
{
    public static class PipelineContextExtensions
    {
        public static IterableDataSettings GetIterableDataSettings(this PipelineContext pipelineContext)
        {
            return pipelineContext.GetPlugin<IterableDataSettings>();
        }

        public static bool HasIterableDataSettings(this PipelineContext pipelineContext)
        {
            return (GetIterableDataSettings(pipelineContext) != null);
        }

        public static ItemMetadataSettings GetItemMetadataSettings(this PipelineContext pipelineContext)
        {
            return pipelineContext.GetPlugin<ItemMetadataSettings>();
        }

        public static bool HasItemMetadataSettings(this PipelineContext pipelineContext)
        {
            return (GetItemMetadataSettings(pipelineContext) != null);
        }
    }
}