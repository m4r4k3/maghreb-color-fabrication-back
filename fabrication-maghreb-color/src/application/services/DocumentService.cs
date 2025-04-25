using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Infrastructure.dto;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.Infrastructure.Repositories;

namespace fabrication_maghreb_color.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly SageOM _SageOm;

        public DocumentService(IDocumentRepository documentRepository, SageOM sageOm)
        {
            _SageOm = sageOm;
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
        public async Task TransformDocument(DocumentDto document)
        {
            try
            {
                await _SageOm.TransformDocument(document);
            }
            catch (Exception ex)
            {
                throw new Exception("Error transforming document", ex);
            }
        }
    }
}
