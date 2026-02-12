using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Core.Models.Student;
using Telerik.Documents.Core.Fonts;
using Telerik.Documents.Primitives;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Model;
using RgbColor = Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor;

namespace KendoMvcDemo.Infrastructure.Documents
{
    public class StudentsPdfDocumentGenerator : IPdfDocumentGenerator<StudentExportDto>
    {
        private const double defaultRowHeight = 60;

        public StudentsPdfDocumentGenerator()
        {
        }

        public byte[] Export(params IEnumerable<StudentExportDto> data)
        {
            // Exporting HTML to PDF
            //string htmlContent = "<!DOCTYPE html><html><body><p>Hello, world!</p></body></html>";

            //Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider htmlProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Html.HtmlFormatProvider();
            //// Create a document instance from the content. 
            //Telerik.Windows.Documents.Flow.Model.RadFlowDocument document = htmlProvider.Import(htmlContent, TimeSpan.FromSeconds(10));

            //Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();

            //// Export the document. The different overloads enables you to export to a byte[] or to a Stream. 
            //byte[] pdfBytes = pdfProvider.Export(document, TimeSpan.FromSeconds(10));

            //return pdfBytes;

            return ExportInternal(data);
        }

        private byte[] ExportInternal(IEnumerable<StudentExportDto> data)
        {
            RadFixedDocument document = new();

            document.ViewerPreferences.ShouldDisplayDocumentTitle = false;
            document.ViewerPreferences.ShouldFitWindow = true;

            using RadFixedDocumentEditor editor = new(document);

            editor.SectionProperties.PageMargins = new Thickness(96);   // Default is 96
            editor.SectionProperties.PageSize = PaperTypeConverter.ToSize(PaperTypes.A4);

            Block titleBlock = new();
            titleBlock.TextProperties.FontSize = 24;
            titleBlock.InsertText("Students PDF Export");

            Block subTitleBlock = new();
            FontsRepository.TryCreateFont(new FontFamily("Qahiri"), out FontBase qahiriFont);
            subTitleBlock.TextProperties.Font = qahiriFont;
            subTitleBlock.TextProperties.FontSize = 18;

            subTitleBlock.InsertText("أهلا بكم في تيليريك");

            editor.InsertBlock(titleBlock);
            editor.InsertBlock(subTitleBlock);

            editor.InsertLineBreak();
            editor.InsertLineBreak();

            DrawStudentsTable(editor, data);

            using MemoryStream ms = new();

            PdfFormatProvider pdfFormatProvider = new();
            pdfFormatProvider.Export(document, ms, timeout: TimeSpan.FromSeconds(10));

            return ms.ToArray();
        }

        private void DrawStudentsTable(RadFixedDocumentEditor editor, IEnumerable<StudentExportDto> data)
        {
            Table table = new()
            {
                Borders = new TableBorders(new(BorderStyle.Single)),
                LayoutType = TableLayoutType.FixedWidth,
                HorizontalAlignment = TableHorizontalAlignment.Left
            };

            table.DefaultCellProperties.Background = new RgbColor(224, 224, 224);
            table.DefaultCellProperties.Borders = new TableCellBorders(
                new Border(1, new RgbColor(0,0,0)), 
                new Border(1, new RgbColor(0,0,0)),
                new Border(1, new RgbColor(0,0,0)),
                new Border(1, new RgbColor(0,0,0)));
            table.DefaultCellProperties.Padding = new Thickness(10);

            TableRow tableHeaderRow = AddTableRow(table);
            ColorBase tableHeaderRowBackground = new RgbColor(156, 39, 176);

            AddTableCell(tableHeaderRow, "Id", tableHeaderRowBackground);
            AddTableCell(tableHeaderRow, "Name", tableHeaderRowBackground);
            AddTableCell(tableHeaderRow, "Student Number", tableHeaderRowBackground);
            AddTableCell(tableHeaderRow, "Avatar", tableHeaderRowBackground);

            foreach (StudentExportDto student in data)
            {
                TableRow tableRow = AddTableRow(table);

                AddTableCell(tableRow, student.Id.ToString());
                AddTableCell(tableRow, student.Name);
                AddTableCell(tableRow, student.StudentNumber);

                using (MemoryStream ms = new(student.AvatarImageData))
                {
                    TableCell cell = AddTableCell(tableRow, student.StudentNumber, isText: false);
                    AddImageToCell(cell, ms);
                }
            }

            editor.InsertTable(table);
        }

        private TableRow AddTableRow(Table table)
        {
            TableRow row = table.Rows.AddTableRow();
            
            row.Height = new TableRowHeight(HeightType.Exact, defaultRowHeight);

            return row;
        }

        private TableCell AddTableCell(
            TableRow tableRow, 
            string text,
            ColorBase cellBackground = null,
            bool isText = true)
        {
            TableCell cell = tableRow.Cells.AddTableCell();

            cell.VerticalAlignment = VerticalAlignment.Center;

            if (cellBackground != null)
            {
                cell.Background = cellBackground;
            }

            if (isText)
            {
                AddTextToCell(cell, text);
            }

            return cell;
        }

        private void AddTextToCell(TableCell cell, string text)
        {
            Block block = cell.Blocks.AddBlock();
            block.InsertText(text);
        }

        private void AddImageToCell(TableCell cell, Stream imageStream)
        {
            Image img = new();

            ImageSource imageSrc = new(imageStream);
            img.ImageSource = imageSrc;
            img.Width = 40;
            img.Height = 40;
            img.AlphaConstant = 0.5;
            SimplePosition simplePosition = new();
            img.Position = simplePosition;

            cell.Blocks.AddBlock().InsertImage(imageSrc, new Size(40, 40));
        }

        public byte[] ExportDocxToPdf(Stream pdfStream)
        {
            throw new NotImplementedException();
        }
    }
}
