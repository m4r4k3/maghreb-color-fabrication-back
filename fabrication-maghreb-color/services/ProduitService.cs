using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.model;


namespace fabrication_maghreb_color.service
{
    public class ProduitService
    {
        private readonly MainContext _dbContext;
        private readonly ILogger<ProduitService> _logger;
        public ProduitService(MainContext context, ILogger<ProduitService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        public List<Produit> GetAll()
        {
            return _dbContext.ProduitDbo.ToList();
        }
        public void Create(Produit produit){
            _dbContext.ProduitDbo.Add(produit) ;
            _dbContext.SaveChanges( ) ;
        }
    }
}