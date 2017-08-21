using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Contexts
{
    public class PageContext : BaseDataExchangeContext
    {
        public ItemMetadata Metadata { get; set; }

        public ItemDetail ItemDetail { get; set; }

        public HtmlDocument Document { get; set; }
    }
}