using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Models
{
    public class Page : BaseHasPlugins
    {
        public IPageProcessor PageProcessor { get; set; }
    }
}
