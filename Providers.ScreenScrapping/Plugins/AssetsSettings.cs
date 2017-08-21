using System.Collections;
using System.Collections.Generic;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class AssetsSettings
        : Sitecore.DataExchange.IPlugin
    {
        public AssetsSettings()
        {
        }

        public IList<Field> Fields { get; set; }

        public IList<IAssetInfo> Assets { get; set; }
    }
}
