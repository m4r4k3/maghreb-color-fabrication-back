using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{

    public class DocumentRepository : IDocumentRepository
    {
        private readonly SageContext _dbContext;

        public DocumentRepository(SageContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Document> GetAllDocuments()
        {
            return _dbContext.Documents.ToList();
        }
        public List<Document> GetAllDocumentsByType(short type)
        {
            return _dbContext.Documents.Where(d => d.DocumentType.Equals( type)).Include(e=>e.DocumentLines).Include(e=>e.ChargeCompte).Include(e=>e.Customer).ToList();
        }
    }
}