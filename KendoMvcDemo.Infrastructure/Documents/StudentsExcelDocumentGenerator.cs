using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Core.Models.Student;
using System.IO;
using Telerik.Documents.Common.Model;
using Telerik.Documents.Media;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace KendoMvcDemo.Infrastructure.Documents
{
    public class StudentsExcelDocumentGenerator : IExcelDocumentGenerator<StudentExportDto>
    {
        public byte[] Export(params IEnumerable<StudentExportDto> data)
        {
            return ExportInternal(data);
        }

        private byte[] ExportInternal(IEnumerable<StudentExportDto> data)
        {
            Workbook workbook = new();

            Worksheet worksheet = workbook.Worksheets.Add();
            worksheet.Name = "Students";

            BuildHeader(worksheet);
            BuildBody(worksheet, data);

            worksheet.Rows.SetDefaultHeight(new RowHeight(60, isCustom: true));

            // Auto-resize all columns to fit content
            //ColumnSelection columnSelection = worksheet.Columns[0, 4];
            //columnSelection.AutoFitWidth();

            worksheet.Columns.SetDefaultWidth(new ColumnWidth(200, isCustom: true));
            worksheet.Columns[0].SetWidth(new ColumnWidth(50, isCustom: true));
            worksheet.Columns[0].SetHorizontalAlignment(RadHorizontalAlignment.Left);
            worksheet.Columns[3].SetWidth(new ColumnWidth(60, isCustom: true));

            XlsxFormatProvider formatProvider = new();
            using MemoryStream ms = new();
            formatProvider.Export(workbook, ms, timeout: null);

            return ms.ToArray();
        }

        private void BuildHeader(Worksheet worksheet)
        {
            worksheet.Cells[0, 0].SetValue("Id");
            worksheet.Cells[0, 1].SetValue("Name");
            worksheet.Cells[0, 2].SetValue("Student Number");
            worksheet.Cells[0, 3].SetValue("Avatar");

            // Select range for all header cells.
            worksheet.Cells[0, 0, 0, 3].SetIsBold(true);
            worksheet.Cells[0, 0, 0, 3].SetVerticalAlignment(RadVerticalAlignment.Center);

            worksheet.Cells[0, 0, 0, 3].SetFill(
                new GradientFill(GradientType.Horizontal, Color.FromArgb(255, 46, 204, 113), Color.FromArgb(255, 0, 134, 56)));

            worksheet.Cells[0, 0, 0, 3].SetBorders(
                new CellBorders(
                    left: new CellBorder(CellBorderStyle.Thick, new ThemableColor(Colors.Black)),
                    top: new CellBorder(CellBorderStyle.Thick, new ThemableColor(Colors.Black)),
                    right: new CellBorder(CellBorderStyle.Thick, new ThemableColor(Colors.Black)),
                    bottom: new CellBorder(CellBorderStyle.Thick, new ThemableColor(Colors.Black)),
                    insideHorizontal: CellBorder.Default,
                    insideVertical: CellBorder.Default,
                    diagonalUp: CellBorder.Default,
                    diagonalDown: CellBorder.Default));
        }

        private void BuildBody(Worksheet worksheet, IEnumerable<StudentExportDto> data)
        {
            int rowIndex = 1;

            //List<string> headers = new['Id', 'Name', 'Student Number', 'Avatar'];
            //Dictionary<int, List<object>> studentsDict = [];
            //studentsDict[1] = new
            //{
            //   { 1, "Alex Ivanov", "FN-001", new byte[0] { } }
            //};

            foreach (StudentExportDto student in data)
            {
                int columnIndex = 0;
                worksheet.Cells[rowIndex, columnIndex].SetValue("");

                worksheet.Cells[rowIndex, 0].SetValue(student.Id);
                worksheet.Cells[rowIndex, 1].SetValue(student.Name);
                worksheet.Cells[rowIndex, 2].SetValue(student.StudentNumber);

                FloatingImage image = new(worksheet, new CellIndex(rowIndex, 3), 10, 10)
                {
                    Width = 40,
                    Height = 40,
                    RotationAngle = 0
                };

                using (MemoryStream ms = new(student.AvatarImageData))
                {
                    image.ImageSource = new ImageSource(ms, "jpg");
                }

                worksheet.Images.Add(image);

                worksheet.Cells[rowIndex, 0, rowIndex, 3].SetBorders(
                    new CellBorders(
                        left: new CellBorder(CellBorderStyle.Thin, new ThemableColor(Colors.Black)),
                        top: new CellBorder(CellBorderStyle.Thin, new ThemableColor(Colors.Black)),
                        right: new CellBorder(CellBorderStyle.Thin, new ThemableColor(Colors.Black)),
                        bottom: new CellBorder(CellBorderStyle.Thin, new ThemableColor(Colors.Black)),
                        insideHorizontal: CellBorder.Default,
                        insideVertical: CellBorder.Default,
                        diagonalUp: CellBorder.Default,
                        diagonalDown: CellBorder.Default));

                worksheet.Cells[rowIndex, 0, rowIndex, 3].SetVerticalAlignment(RadVerticalAlignment.Center);

                rowIndex++;
            }
        }
    }
}
