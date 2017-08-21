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
    public class GenericContentReaderStepProcessor : BasePageReadStepProessor
    {
        protected override void ReadContent(HtmlDocument document, ItemMetadata metadata, ItemDetail itemDetail)
        {
            var body = document.DocumentNode.SelectNodes("//main").GetFirstNode();
            itemDetail.Fields["Body"] = body.GetInnerHtml();
        }
    }
}
