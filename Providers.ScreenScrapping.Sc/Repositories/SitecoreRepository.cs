using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Repositories;
using Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Search;
using Sitecore.Globalization;
using Sitecore.SecurityModel;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Repositories
{
    public class SitecoreRepository : IContentRepository
    {
        private void SaveFieldValue(Item storeItem, string fieldName, string fieldValue)
        {
            Data.Fields.Field field = null;
            field = ID.IsID(fieldName) ? storeItem.Fields[new ID(fieldName)] : storeItem.Fields[fieldName];

            if (field == null)
                return;

            field.Value = fieldValue;
        }

        public ItemNameInfo GetName(string name)
        {
            var result = new ItemNameInfo
            {
                OriginalItemName = name,
                IsItemNameValid = ItemUtil.IsItemNameValid(name),
                ProposedItemName = name
            };

            if (!result.IsItemNameValid)
            {
                result.ProposedItemName = ItemUtil.ProposeValidItemName(name);
            }

            return result;
        }

        public void Save(CreateUpdateItemInfo info)
        {
            var language = !string.IsNullOrEmpty(info.Metadata.LanguageName) ? Language.Parse(info.Metadata.LanguageName) :
            LanguageManager.DefaultLanguage;
            var databaseName = string.IsNullOrEmpty(info.Metadata.DatabaseName) ? "master" : info.Metadata.DatabaseName;
            var database = Factory.GetDatabase(databaseName);
            var parentItemId = info.Metadata.ParentItemId;
            var templateId = info.Metadata.TemplateId;
            var itemName = info.Metadata.ItemName;

            using (new SecurityDisabler())
            {
                Item parentItem = database.GetItem(new ID(parentItemId), language);

                Item itmTemplate = database.GetItem(new ID(templateId));

                TemplateItem template = database.GetTemplate(itmTemplate.ID);

                var newItem = ItemNameSearch.GetItem(parentItem, template, language, itemName);

                //var newItem = parentItem.Axes.GetDescendant(itemName);

                var itemAction = "update";
                if (newItem == null)
                {
                    newItem = parentItem.Add(itemName, template);
                    itemAction = "insert";
                }

                info.Metadata.ItemAction = itemAction;
                info.Metadata.ItemId = newItem.ID.ToString();

                using (new EditContext(newItem))
                {
                    if (info.Fields != null && info.Fields.Any())
                    {
                        foreach (var infoFieldValue in info.Fields)
                        {
                            SaveFieldValue(newItem, infoFieldValue.Key, infoFieldValue.Value.ToString());
                        }
                    }
                }

                return;
            }
        }

        public IList<string> ResolveFieldValue(FieldValueSearchOptions fieldValueSearchOptions)
        {
            var t = Type.GetType(fieldValueSearchOptions.TypeFullName);
            var instance = Activator.CreateInstance(t) as FieldValueSearch;
            if (instance != null)
            {
                var items = instance.GetItems(fieldValueSearchOptions);

                if (items != null && items.Any())
                {
                    return items.Select(x => x.ID.ToString()).ToList();
                }
            }

            return null;
        }

        public void SaveMedia(CreateUpdateMediaInfo info, string fileName, Stream stream)
        {
            Sitecore.Resources.Media.MediaCreatorOptions options = new Sitecore.Resources.Media.MediaCreatorOptions
            {
                Database = !string.IsNullOrEmpty(info.DatabaseName) ? Factory.GetDatabase(info.DatabaseName)
                : Factory.GetDatabase("master")
            };

            if (!string.IsNullOrEmpty(info.LanguageName))
            {
                options.Language = Sitecore.Globalization.Language.Parse(info.LanguageName);
            }

            var parentItem = options.Database.GetItem(info.ParentId);

            options.Destination = parentItem.Paths.FullPath.Trim().TrimEnd('/') + "/" + ItemUtil.ProposeValidItemName(info.MediaName);

            // Now create the file in Sitecore
            Item mediaItem = Sitecore.Resources.Media.MediaManager.Creator.CreateFromStream(stream, fileName, options);
            
            info.MediaId = mediaItem.ID.ToString();
        }
    }
}