using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Core.Models.Student;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Documents.Core.Fonts;
using Telerik.Documents.Common.Model;
using Telerik.Documents.Media;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Flow.Model.Shapes;

namespace KendoMvcDemo.Infrastructure.Documents
{
    public class StudentsWordDocumentGenerator : IWordDocumentGenerator<StudentExportDto>
    {
        public byte[] Export(params IEnumerable<StudentExportDto> data)
        {
            return ExportInternal(data);
        }

        private byte[] ExportInternal(IEnumerable<StudentExportDto> data)
        {
            RadFlowDocument document = new();
            RadFlowDocumentEditor editor = new(document);

            Section layoutSection = document.Sections.AddSection();
            layoutSection.PageSize = PaperTypeConverter.ToSize(PaperTypes.A4);

            Header header = layoutSection.Headers.Add();
            Paragraph paragraph = header.Blocks.AddParagraph();
            paragraph.TextAlignment = Alignment.Center;
            Run headerText = paragraph.Inlines.AddRun("Students Export");
            headerText.FontSize = 36;

            BuildTable(editor, data);

            using MemoryStream ms = new();
            DocxFormatProvider provider = new();

            provider.Export(document, ms, timeout: null);

            return ms.ToArray();
        }

        private void BuildTable(RadFlowDocumentEditor editor, IEnumerable<StudentExportDto> students)
        {
            Table table = editor.InsertTable();
            ThemableColor cellBackground = new(Colors.Gray);

            TableRow headerRow = table.Rows.AddTableRow();

            AddTableCellWithText(headerRow, "Id", 50, cellBackground);
            AddTableCellWithText(headerRow, "Name", 150, cellBackground);
            AddTableCellWithText(headerRow, "Student Number", 150, cellBackground);
            AddTableCellWithText(headerRow, "Avatar", 60, cellBackground);

            foreach (StudentExportDto student in students)
            {
                TableRow row = table.Rows.AddTableRow();

                AddTableCellWithText(row, student.Id.ToString(), 50);
                AddTableCellWithText(row, student.Name, 150);
                AddTableCellWithText(row, student.StudentNumber, 150);
                AddTableCellWithImage(row, student.AvatarImageData, 60);
            }

            editor.InsertParagraph();
        }

        private TableCell AddTableCellWithText(
            TableRow tableRow,
            string text,
            double? width = null,
            ThemableColor cellBackground = null)
        {
            TableCell cell = AddTableCell(tableRow, width, cellBackground);      
            cell.Blocks.AddParagraph().Inlines.AddRun(text);

            return cell;
        }

        private TableCell AddTableCellWithImage(
            TableRow tableRow,
            byte[] imageData,
            double? width = null,
            ThemableColor cellBackground = null)
        {
            TableCell cell = AddTableCell(tableRow, width, cellBackground);
            ImageInline imageInline = cell.Blocks.AddParagraph().Inlines.AddImageInline();

            imageInline.Image.ImageSource = new(imageData, "jpg");
            imageInline.Image.Width = 40;
            imageInline.Image.Height = 40;

            return cell;
        }

        private TableCell AddTableCell(
            TableRow tableRow,
            double? width = null,
            ThemableColor cellBackground = null)
        {
            TableCell cell = tableRow.Cells.AddTableCell();
            cell.VerticalAlignment = VerticalAlignment.Center;

            if (width != null)
            {
                cell.PreferredWidth = new TableWidthUnit(width.Value);
            }

            if (cellBackground != null)
            {
                cell.Shading.BackgroundColor = cellBackground;
            }

            return cell;
        }
    }
}
