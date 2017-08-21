using HtmlAgilityPack;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction
{
    public interface IAssetInfo
    {
        bool Enabled { get; set; }

        IAssetProcessor ProcessorType { get; set; }
    }
}