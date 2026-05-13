using System.Data;

namespace KendoMvcDemo.Core.Documents
{
    public interface IDocumentGenerator
    {
        byte[] Export(DataTable data);
    }

    public interface IDocumentGenerator<TModel>
    {
        byte[] Export(params IEnumerable<TModel> data);
    }
}
