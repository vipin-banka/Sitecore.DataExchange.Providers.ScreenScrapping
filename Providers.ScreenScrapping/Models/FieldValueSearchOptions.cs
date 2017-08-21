using System.Collections.Generic;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Models
{
    public class FieldValueSearchOptions
    {
        public FieldValueSearchOptions()
        {
            ParentItems = new List<string>();
            Templates = new List<string>();
            Values = new List<string>();
        }

        public string IndexName { get; set; }

        public IList<string> ParentItems { get; set; }

        public IList<string> Templates { get; set; }

        public string LanguageName { get; set; }

        public bool SearchByName { get; set; }

        public IList<string> Values { get; set; }

        public string TypeFullName { get; set; }

    }
}
