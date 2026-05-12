namespace KendoMvcDemo.Core.Documents
{
    public interface IPdfDocumentGenerator
    {
        byte[] ExportDocxToPdf(Stream stream);
    }

    public interface IPdfDocumentGenerator<TModel> : IPdfDocumentGenerator, IDocumentGenerator<TModel>
    {
    }
}
