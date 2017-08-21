using System;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Search;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Pluralsight.Sc.FieldResolvers
{
    public class DefaultFieldResolver : FieldValueSearch
    {
        protected override Expression<Func<SearchResultItem, bool>> AddFilters(FieldValueSearchOptions fieldValueSearchOptions)
        {
            return null;
        }
    }
}
