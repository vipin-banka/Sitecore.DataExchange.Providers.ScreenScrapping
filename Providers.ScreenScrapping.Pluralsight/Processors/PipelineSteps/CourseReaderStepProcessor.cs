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
    public class CourseReaderStepProcessor : BasePageReadStepProessor
    {
        protected override void ReadContent(HtmlDocument document, ItemMetadata metadata, ItemDetail itemDetail)
        {
            var title = document.DocumentNode.SelectNodes("//h1").GetFirstNode();
            var shortDescription = document.DocumentNode.SelectNodes("//div[@class='text-component']").GetFirstNode();
            var longDescription = document.DocumentNode.SelectNodes("//div[@class='course-info-tile-right']").GetFirstNode();
            var author = document.DocumentNode.SelectNodes("//div[@class='author_image']").GetFirstNode();

            itemDetail.Fields["Title"] = title.GetInnerText();
            itemDetail.Fields["ShortDescription"] = shortDescription.GetInnerHtml();
            itemDetail.Fields["Body"] = longDescription.GetInnerHtml();
            itemDetail.ResolveFields["Authors"] = new List<string> { author.GetInnerText() };
        }
    }
}
