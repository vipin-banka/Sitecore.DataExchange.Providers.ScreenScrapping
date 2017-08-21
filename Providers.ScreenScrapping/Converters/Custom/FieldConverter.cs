using System;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.Custom
{
    public class FieldConverter : BaseItemModelConverter<Field>
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.Custom.FieldTemplateId);

        public FieldConverter(IItemModelRepository repository) : base(repository)
        {
            //
            //identify the template an item must be based
            //on in order for the converter to be able to
            //convert the item
            this.SupportedTemplateIds.Add(TemplateId);
        }

        public override Field Convert(ItemModel source)
        {
            var result = new Field { Name = base.GetStringValue(source, "Name") };
            return result;
        }
    }
}
