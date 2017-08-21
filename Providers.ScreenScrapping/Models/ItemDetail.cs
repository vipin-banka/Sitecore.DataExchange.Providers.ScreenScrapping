using System.Collections.Generic;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Models
{
    public class ItemDetail
    {
        public ItemDetail()
        {
            Metadata = new FieldDictionary();
            Fields = new FieldDictionary();
            ResolveFields = new FieldDictionary();
        }

        public FieldDictionary Metadata { get; set; }
        public FieldDictionary Fields { get; set; }
        public FieldDictionary ResolveFields { get; set; }

        public string Action
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.Action);
            }

            set { SetValue(Metadata, Constants.Properties.Action, value.ToString()); }
        }

        public string ItemId
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.ItemId);
            }

            set { SetValue(Metadata, Constants.Properties.ItemId, value.ToString()); }
        }

        public string TemplateId
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.TemplateId);
            }

            set { SetValue(Metadata, Constants.Properties.TemplateId, value.ToString()); }
        }

        public string ParentItemId
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.ParentItemId);
            }

            set { SetValue(Metadata, Constants.Properties.ParentItemId, value.ToString()); }
        }

        public string ItemName
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.ItemName);
            }

            set { SetValue(Metadata, Constants.Properties.ItemName, value.ToString()); }
        }

        public string DatatabaseName
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.DatabaseName);
            }

            set { SetValue(Metadata, Constants.Properties.DatabaseName, value); }
        }

        public string LanguageName
        {
            get
            {
                return GetValue<string>(Metadata, Constants.Properties.LanguageName);
            }

            set { SetValue(Metadata, Constants.Properties.LanguageName, value); }
        }

        private T GetValue<T>(IDictionary<string, object> values, string key)
        {
            if (values.ContainsKey(key))
                return (T)values[key];
            return default(T);
        }

        private void SetValue(IDictionary<string, object> values, string key, object value)
        {
            if (values.ContainsKey(key))
                values[key] = value;
            values.Add(key, value);
        }

        private bool ContainsKey(IDictionary<string, object> values, string key)
        {
            return values.ContainsKey(key);
        }
    }
}