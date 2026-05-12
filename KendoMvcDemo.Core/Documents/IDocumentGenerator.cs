namespace KendoMvcDemo.Core.Documents
{
    public interface IDocumentGenerator<TModel>
    {
        byte[] Export(params IEnumerable<TModel> data);
    }
}
