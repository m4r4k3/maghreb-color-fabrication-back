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

            return _documentRepository.GetAllDocuments();

        }
        public List<Document> GetAllDocumentsByType(short type)
        {

            return _documentRepository.GetAllDocumentsByType(type);

        }
        public async Task TransformDocument(DocumentDto document)
        {

            await _SageOm.TransformDocument(document);

        }
    }
}
