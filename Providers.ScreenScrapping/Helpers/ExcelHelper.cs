using System;
using System.Data;
using System.IO;
using System.Linq;
using DataTable = System.Data.DataTable;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Helpers
{
    public class ExcelHelper
    {
        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true, string sheetName = "")
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                
                var ws = pck.Workbook.Worksheets.First();

                if (!string.IsNullOrEmpty(sheetName))
                {
                    ws = pck.Workbook.Worksheets.FirstOrDefault(p => p.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
                }

                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }

                return tbl;
            }
        }
    }
}