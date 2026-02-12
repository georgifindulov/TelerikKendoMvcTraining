namespace KendoMvcDemo.Core.Documents
{
    public interface IPdfDocumentGenerator
    {
        byte[] ExportDocxToPdf(Stream stream);
    }

    public interface IPdfDocumentGenerator<TModel> : IPdfDocumentGenerator
    {
        byte[] Export(params IEnumerable<TModel> data);
    }
}
