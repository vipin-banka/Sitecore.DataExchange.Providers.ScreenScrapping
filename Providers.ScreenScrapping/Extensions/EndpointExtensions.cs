using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Extensions
{
    public static class EndpointExtensions
    {
        public static ExcelFileSettings GetExcelFileSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<ExcelFileSettings>();
        }

        public static bool HasExcelFileSettings(this Endpoint endpoint)
        {
            return (GetExcelFileSettings(endpoint) != null);
        }

        public static SitemapSettings GetSitemapSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<SitemapSettings>();
        }

        public static bool HasSitemapSettings(this Endpoint endpoint)
        {
            return (GetSitemapSettings(endpoint) != null);
        }
        public static SitecoreRepositorySettings GetSitecoreRepositorySettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<SitecoreRepositorySettings>();
        }

        public static bool HasSitecoreRepositorySettings(this Endpoint endpoint)
        {
            return (GetSitecoreRepositorySettings(endpoint) != null);
        }

        public static IndexNameSettings GetIndexNameSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<IndexNameSettings>();
        }

        public static bool HasIndexNameSettings(this Endpoint endpoint)
        {
            return (GetIndexNameSettings(endpoint) != null);
        }
    }
}
