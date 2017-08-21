using Sitecore.Configuration;
using Sitecore.DataExchange.Providers.ScreenScrapping.Repositories;
using Sitecore.Pipelines;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Sc.Pipelines.Loader
{
    public class InitializeDataExchange
    {
        public void Process(PipelineArgs args)
        {
            CustomContext.Repository = Factory.CreateObject("dataExchange/ScreenScrappingRepository", true) as IContentRepository;
        }
    }
}