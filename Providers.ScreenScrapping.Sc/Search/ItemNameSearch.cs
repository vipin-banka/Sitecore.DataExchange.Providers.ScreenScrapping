using System;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Search
{
    public static class ItemNameSearch
    {
        public static Item GetItem(Item parentItem, TemplateItem template, Language language, string itemName)
        {
            var index = ContentSearchManager.GetIndex("sitecore_master_index");
            using (var context = index.CreateSearchContext())
            {
                var query = context.GetQueryable<SearchResultItem>()
                .Filter(x => x.Language == language.Name
                             && x.Paths.Contains(parentItem.ID)
                             && x.TemplateId == template.ID
                             && x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

                var searchedItem = query.FirstOrDefault();
                if (searchedItem != null)
                {
                    return searchedItem.GetItem();
                }
            }

            return null;
        }
    }
}
