using KendoMvcDemo.Core.Documents;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.Model;

namespace KendoMvcDemo.Infrastructure.Documents
{
    public class DefaultPdfDocumentGenerator : IPdfDocumentGenerator
    {
        public byte[] ExportDocxToPdf(Stream stream)
        {
            // Import DOCX
            DocxFormatProvider fileFormatProvider = new();
            RadFlowDocument document = fileFormatProvider.Import(stream, timeout: null);

            // Export PDF
            PdfFormatProvider pdfProvider = new();

            using MemoryStream pdfStream = new();
            pdfProvider.Export(document, pdfStream, timeout: null);

            return pdfStream.ToArray();
        }
    }
}
