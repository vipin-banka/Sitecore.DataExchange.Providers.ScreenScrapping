using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class WebPageReaderSettings : Sitecore.DataExchange.IPlugin
    {
        public WebPageReaderSettings()
        {
        }

        public IWebPageReader Reader { get; set; }

        public string ParentItemId { get; set; }

        public string TemplateId { get; set; }

        public string DatabaseName { get; set; }
    }
}