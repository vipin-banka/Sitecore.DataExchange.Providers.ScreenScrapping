using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class FieldValueResolverSettings : FieldValueSearchOptions, Sitecore.DataExchange.IPlugin
    {
        public string Key { get; set; }
    }
}