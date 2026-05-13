using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Core.Models.Student;
using System.Data;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace KendoMvcDemo.Infrastructure.Documents
{
    public class ExcelDynamicGenerator : IExcelDocumentGenerator
    {
        public byte[] Export(DataTable data)
        {
            return ExportInternal(data);
        }

        private byte[] ExportInternal(DataTable data)
        {
            Workbook workbook = new();

            Worksheet worksheet = workbook.Worksheets.Add();
            worksheet.Name = "Dynamic Data Table";

            worksheet.Rows.SetDefaultHeight(new RowHeight(60, isCustom: true));

            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    CellSelection cell = worksheet.Cells[row, col];
                    object value = data.Rows[row][col];

                    if (value == DBNull.Value || value == null)
                    {
                        continue;
                    }

                    // Set value by type for correct Excel cell types
                    switch (value)
                    {
                        case int i: cell.SetValue(i); break;
                        case long l: cell.SetValue(l); break;
                        case double d: cell.SetValue(d); break;
                        case float f: cell.SetValue(f); break;
                        case bool b: cell.SetValue(b); break;
                        case DateTime dt: cell.SetValue(dt); break;
                        default: cell.SetValue(value.ToString()); break;
                    }

                }
            }

            // Auto-resize all columns to fit content
            ColumnSelection columnSelection = worksheet.Columns[0, data.Columns.Count];
            columnSelection.AutoFitWidth();

            XlsxFormatProvider formatProvider = new();
            using MemoryStream ms = new();
            formatProvider.Export(workbook, ms, timeout: null);

            return ms.ToArray();
        }
    }
}
