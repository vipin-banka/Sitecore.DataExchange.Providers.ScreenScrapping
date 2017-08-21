using System.Data;
using System.IO;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Helpers;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    //[RequiredEndpointPlugins(typeof(ExcelFileSettings))]
    public class ReadExcelFileStepProcessor : BaseMetadataStepProcessor
    {
        protected override void ReadMetadata(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            //get the file path from the plugin on the endpoint
            var settings = endpoint.GetExcelFileSettings();
            if (settings == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(settings.Path))
            {
                Logger.Error(
                    "No path is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }

            if (!File.Exists(settings.Path))
            {
                Logger.Error(
                    "The path specified on the endpoint does not exist. " +
                    "(pipeline step: {0}, endpoint: {1}, path: {2})",
                    pipelineStep.Name, endpoint.Name, settings.Path);
                return;
            }

            //read the file, one line at a time
            var dataTable = ExcelHelper.GetDataTableFromExcel(settings.Path, true,
                settings.SheetName);

            DataRowCollection rows = null;

            if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataTableRow in dataTable.Rows)
                {
                    this.AddMetadata(itemMetadata => GetItemMetadata(dataTableRow, itemMetadata));
                }
            }

            Logger.Info("{0} rows were read from the file. (pipeline step: {1}, endpoint: {2})",
                dataTable.Rows.Count, pipelineStep.Name, endpoint.Name);
        }

        protected bool GetItemMetadata(DataRow dataRow, ItemMetadata itemMetadata)
        {
            itemMetadata.Url = dataRow["Url"].ToString();
            itemMetadata.LanguageName = dataRow["Lang"].ToString();
            itemMetadata.ItemName = dataRow["ItemName"].ToString();
            itemMetadata.PageType = dataRow["PageType"].ToString();
            return true;
        }
    }
}