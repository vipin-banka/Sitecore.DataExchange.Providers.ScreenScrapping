using Sitecore.DataExchange.Providers.ScreenScrapping.Repositories;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class SitecoreRepositorySettings : Sitecore.DataExchange.IPlugin
    {
        public SitecoreRepositorySettings()
        {
        }

        public IContentRepository Repository { get; set; }
    }
}
