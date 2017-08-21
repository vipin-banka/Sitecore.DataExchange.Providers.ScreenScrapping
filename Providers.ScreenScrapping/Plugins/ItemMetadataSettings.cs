using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class ItemMetadataSettings : Sitecore.DataExchange.IPlugin
    {
        public ItemMetadataSettings(ItemMetadata itemMetadata)
        {
            ItemMetadata = itemMetadata;
            ItemDetail = new ItemDetail();
        }

        public ItemDetail ItemDetail { get; set; }
        public ItemMetadata ItemMetadata { get; set; }
    }
}