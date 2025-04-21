using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IDocumentService
    {
        List<Document> GetAllDocuments();
        List<Document> GetAllDocumentsByType(short type);
    
    }
}