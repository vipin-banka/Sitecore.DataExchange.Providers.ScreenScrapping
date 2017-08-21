using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Extensions
{
    public static class PipelineBatchContextExtensions
    {
        public static MetadataSettings GetMetadataSettings(this PipelineBatchContext pipelineBatchContext)
        {
            return pipelineBatchContext.GetPlugin<MetadataSettings>();
        }

        public static bool HasMetadataSettings(this PipelineBatchContext pipelineBatchContext)
        {
            return (GetMetadataSettings(pipelineBatchContext) != null);
        }
    }
}