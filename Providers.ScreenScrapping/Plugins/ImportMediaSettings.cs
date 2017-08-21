using System.Collections;
using System.Collections.Generic;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class ImportMediaSettings
        : Sitecore.DataExchange.IPlugin
    {
        public ImportMediaSettings()
        {
            Fields = new List<Field>();
        }

        public IList<Field> Fields { get; set; }

        public string Extensions { get; set; }

        public string ParentItem { get; set; }

        public string BaseUrl { get; set; }

        public string DownloadFolderPath { get; set; }
    }
}
