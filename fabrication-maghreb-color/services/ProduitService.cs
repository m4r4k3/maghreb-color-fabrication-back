using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.service
{
    public class ProduitService
    {
        private readonly SageContext _SageDbContext;
        private readonly ILogger<ProduitService> _logger;
        public ProduitService(SageContext context, ILogger<ProduitService> logger)
        {
            _SageDbContext = context;
            _logger = logger;
        }
              public List<SageArticle> GetAll()
        {
            return _SageDbContext.SageArticleDbo.Where(e => EF.Functions.Like(e.Reference, "PF%")).ToList();
        }
    }
}