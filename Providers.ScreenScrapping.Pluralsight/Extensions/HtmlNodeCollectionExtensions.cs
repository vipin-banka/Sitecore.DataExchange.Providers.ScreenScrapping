using System.Linq;
using HtmlAgilityPack;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Pluralsight.Extensions
{
    public static class HtmlNodeCollectionExtensions
    {
        public static HtmlNode GetFirstNode(this HtmlNodeCollection collection)
        {
            if (collection == null)
                return null;
            return collection.FirstOrDefault();
        }

        public static string GetInnerText(this HtmlNode htmlNode)
        {
            if (htmlNode == null)
                return string.Empty;
            return htmlNode.InnerText.Trim();
        }

        public static string GetInnerHtml(this HtmlNode htmlNode)
        {
            if (htmlNode == null)
                return string.Empty;
            return htmlNode.InnerHtml.Trim();
        }
    }
}