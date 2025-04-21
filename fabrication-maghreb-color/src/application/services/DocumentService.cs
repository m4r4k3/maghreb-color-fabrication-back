using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.Infrastructure.Repositories;

namespace fabrication_maghreb_color.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {


            _documentRepository = documentRepository;
        }

    
        public List<Document> GetAllDocuments()
        {
            try
            {
                return _documentRepository.GetAllDocuments();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving documents", ex);
            }
        }
    public List<Document> GetAllDocumentsByType(short type)
        {
            try
            {
                return _documentRepository.GetAllDocumentsByType(type);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving documents by type", ex);
            }
        }
    }
}