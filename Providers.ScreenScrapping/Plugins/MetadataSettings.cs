using System.Collections;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class MetadataSettings : Sitecore.DataExchange.IPlugin
    {
        public MetadataSettings(IEnumerable items)
        {
            Items = items;
        }

        public IEnumerable Items { get; set; }
    }
}