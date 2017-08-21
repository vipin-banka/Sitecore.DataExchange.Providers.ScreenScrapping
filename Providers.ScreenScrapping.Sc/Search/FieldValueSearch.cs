using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.Globalization;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Search
{
    public abstract class FieldValueSearch
    {
        public IList<Item> GetItems(FieldValueSearchOptions fieldValueSearchOptions)
        {
            var language = !string.IsNullOrEmpty(fieldValueSearchOptions.LanguageName) ? Language.Parse(fieldValueSearchOptions.LanguageName) :
            LanguageManager.DefaultLanguage;

            var queryPredicate = PredicateBuilder.True<SearchResultItem>();

            var index = ContentSearchManager.GetIndex(fieldValueSearchOptions.IndexName);
            using (var context = index.CreateSearchContext())
            {
                var query = context.GetQueryable<SearchResultItem>();

                queryPredicate = queryPredicate.And(x => x.Language == language.Name);

                if (fieldValueSearchOptions.ParentItems != null && fieldValueSearchOptions.ParentItems.Any())
                {
                    var pathsPredicate = PredicateBuilder.False<SearchResultItem>();
                    foreach (var parentItem in fieldValueSearchOptions.ParentItems)
                    {
                        var id = MapToID(parentItem);
                        if (!ID.IsNullOrEmpty(id))
                        {
                            pathsPredicate = pathsPredicate.Or(x => x.Paths.Contains(id));
                        }
                    }

                    queryPredicate = queryPredicate.And(pathsPredicate);
                }

                if (fieldValueSearchOptions.Templates != null && fieldValueSearchOptions.Templates.Any())
                {
                    var templatesPredicate = PredicateBuilder.False<SearchResultItem>();
                    foreach (var template in fieldValueSearchOptions.Templates)
                    {
                        var id = MapToID(template);
                        if (!ID.IsNullOrEmpty(id))
                        {
                            templatesPredicate = templatesPredicate.Or(x => x.TemplateId == id);
                        }
                    }

                    queryPredicate = queryPredicate.And(templatesPredicate);
                }

                if (fieldValueSearchOptions.SearchByName)
                {
                    var namesPredicate = PredicateBuilder.False<SearchResultItem>();
                    foreach (var value in fieldValueSearchOptions.Values)
                    {
                        namesPredicate = namesPredicate.Or(x => x.Name.Equals(value, StringComparison.OrdinalIgnoreCase));
                    }

                    queryPredicate = queryPredicate.And(namesPredicate);
                }

                var customPredicate = this.AddFilters(fieldValueSearchOptions);
                if (customPredicate != null)
                {
                    queryPredicate = queryPredicate.And(customPredicate);
                }

                query = query.Filter(queryPredicate);
                var searchedItems = query.ToList();
                if (searchedItems.Any())
                {
                    return searchedItems.Select(x => x.GetItem()).ToList();
                }
            }

            return null;
        }

        private ID MapToID(string value)
        {
            if (ID.IsID(value))
            {
                return ID.Parse(value);
            }

            return null;
        }
        protected abstract Expression<Func<SearchResultItem, bool>> AddFilters(FieldValueSearchOptions fieldValueSearchOptions);
    }
}