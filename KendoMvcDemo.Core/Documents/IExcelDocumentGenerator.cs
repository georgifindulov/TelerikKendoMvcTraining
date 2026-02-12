namespace KendoMvcDemo.Core.Documents
{
    public interface IExcelDocumentGenerator<TModel>
    {
        byte[] Export(params IEnumerable<TModel> data);
    }
}
