using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Extensions
{
    public static class PipelineExtensions
    {
        public static PageTypeSettings GetPageTypeSettings(this Pipeline pipeline)
        {
            return pipeline.GetPlugin<PageTypeSettings>();
        }

        public static bool HasPageTypeSettings(this Pipeline pipeline)
        {
            return (GetPageTypeSettings(pipeline) != null);
        }
    }
}