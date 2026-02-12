namespace KendoMvcDemo.Core.Documents
{
    public interface IWordDocumentGenerator<TModel>
    {
        byte[] Export(params IEnumerable<TModel> data);
    }
}
