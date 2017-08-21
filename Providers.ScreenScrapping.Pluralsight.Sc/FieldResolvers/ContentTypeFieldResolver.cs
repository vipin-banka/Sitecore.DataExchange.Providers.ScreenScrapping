using System;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Search;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Pluralsight.Sc.FieldResolvers
{
    public class ContentTypeFieldResolver : FieldValueSearch
    {
        protected override Expression<Func<SearchResultItem, bool>> AddFilters(FieldValueSearchOptions fieldValueSearchOptions)
        {
            if (fieldValueSearchOptions.Values != null && fieldValueSearchOptions.Values.Any())
            {
                var titlePredicate = PredicateBuilder.False<SearchResultItem>();
                foreach (var value in fieldValueSearchOptions.Values)
                {
                    titlePredicate = titlePredicate.Or(x => x["Title"].Equals(value, StringComparison.OrdinalIgnoreCase));
                }

                return titlePredicate;
            }

            return null;
        }
    }
}
