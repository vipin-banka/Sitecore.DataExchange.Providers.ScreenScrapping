using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps;
using HtmlAgilityPack;
using Sitecore.DataExchange.Providers.ScreenScrapping.Pluralsight.Extensions;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Pluralsight.Processors.PipelineSteps
{
    public class BlogReaderStepProcessor : BasePageReadStepProessor
    {
        protected override void ReadContent(HtmlDocument document, ItemMetadata metadata, ItemDetail itemDetail)
        {
            var title = document.DocumentNode.SelectNodes("//h2").GetFirstNode();
            var longDescription = document.DocumentNode.SelectNodes("//div[@class='text-component    ']").GetFirstNode();
            var author = document.DocumentNode.SelectNodes("//meta[@name='authors']").GetFirstNode();
            var publishdate = document.DocumentNode.SelectNodes("//meta[@name='publish-date']").GetFirstNode();

            itemDetail.Fields["Title"] = title.GetInnerText();
            itemDetail.Fields["Body"] = longDescription.GetInnerHtml();
            itemDetail.Fields["PublishDate"] = publishdate.GetAttributeValue("content", string.Empty);

            itemDetail.ResolveFields["Authors"] = new List<string> { author.GetAttributeValue("content", string.Empty) };


        }
    }
}
