using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors;
using Sitecore.DataExchange.Providers.ScreenScrapping.Contexts;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction
{
    public interface IWebPageReader
    {
        bool CanExecute(ItemMetadata metadata, ItemDetail itemDetail);

        void Execute(ItemMetadata metadata, ItemDetail itemDetail);
    }

    public interface IPageProcessor : IDataExchangeProcessor<Page, PageContext>
    {
    }
}