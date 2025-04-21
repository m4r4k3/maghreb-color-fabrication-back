using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IDocumentRepository
    {
        List<Document> GetAllDocuments();
        List<Document> GetAllDocumentsByType(short type);
    }
}