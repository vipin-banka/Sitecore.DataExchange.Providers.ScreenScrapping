using System.Collections.Generic;
using System.IO;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Repositories
{
    public interface IContentRepository
    {
        void Save(CreateUpdateItemInfo info);

        IList<string> ResolveFieldValue(FieldValueSearchOptions fieldValueSearchOptions);

        void SaveMedia(CreateUpdateMediaInfo info, string fileName, Stream stream);
    }
}