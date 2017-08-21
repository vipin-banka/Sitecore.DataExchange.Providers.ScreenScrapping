using System.IO;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Models
{
    public class CreateUpdateItemInfo
    {
        public CreateUpdateItemInfo()
        {
            Metadata = new CreateUpdateMetadata();
            Fields = new FieldDictionary();
        }

        public CreateUpdateMetadata Metadata { get; set; }
        public FieldDictionary Fields { get; set; }
    }

    public class CreateUpdateMetadata
    {
        public string DatabaseName { get; set; }

        public string LanguageName { get; set; }

        public string ParentItemId { get; set; }

        public string TemplateId { get; set; }

        public string ItemName { get; set; }

        public string ItemAction { get; set; }

        public string ItemId { get; set; }
    }

    public class CreateUpdateMediaInfo
    {
        public CreateUpdateMediaInfo()
        {

        }

        public string ParentId { get; set; }

        public string MediaName { get; set; }

        public string DatabaseName { get; set; }

        public string LanguageName { get; set; }

        public string MediaId { get; set; }

    }
}