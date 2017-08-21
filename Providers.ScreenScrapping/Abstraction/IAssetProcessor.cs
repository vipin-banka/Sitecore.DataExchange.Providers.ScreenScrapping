using HtmlAgilityPack;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction
{
    public interface IAssetProcessor
    {
        bool CanExecute(HtmlNode node, IAssetInfo info);

        void Execute(HtmlNode node, IAssetInfo info, CreateUpdateMediaInfo mediaInfo);
    }
}