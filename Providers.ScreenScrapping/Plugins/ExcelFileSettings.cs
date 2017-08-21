namespace Sitecore.DataExchange.Providers.ScreenScrapping.Plugins
{
    public class ExcelFileSettings 
        : Sitecore.DataExchange.IPlugin
    {
        public ExcelFileSettings()
        {
        }

        public string SheetName { get; set; }

        public string Path { get; set; }
    }
}
