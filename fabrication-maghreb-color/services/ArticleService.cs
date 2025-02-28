using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.service
{
    public class ArticleService
    {
        private readonly SageContext _SageDbContext;
        private readonly MainContext _DevContext;
        private readonly ILogger<ArticleService> _logger;
        public ArticleService(SageContext context, MainContext devContext, ILogger<ArticleService> logger)
        {
            _SageDbContext = context;
            _DevContext = devContext;
            _logger = logger;
        }
        public List<SageArticle> GetAllProducts()
        {
            return _SageDbContext.SageArticleDbo.Where(e => EF.Functions.Like(e.Reference, "PF%")).ToList();
        }
        public List<dynamic> GetAllMaterialsByProduct()
        {
            var articles = _SageDbContext.SageArticleDbo
               .Where(e => EF.Functions.Like(e.Reference, "MP%"))
               .ToList();


            var nomenclatures = _DevContext.nomenclatureDbo.ToList();


            var result = articles
      .Join(
          nomenclatures,
          article => article.Reference,
          nomenclature => nomenclature.ReferenceMP,
          (article, nomenclature) => new
          {
              Designation = article.Designation,
              ProduitRef = nomenclature.ReferencePf,
              Quantity = nomenclature.Quantite,
              pourcentage= nomenclature.Pourcentage

          })
      .GroupBy(item => item.ProduitRef)
      .ToDictionary(
          group => group.Key,
          group => group.Select(item => new
          {
              item = item.Designation,
              quantity = item.Quantity,
              pourcentage = item.pourcentage
          }).ToList()
      );
            return result.Select(r => (dynamic)r).ToList();

        }
        public List<dynamic> GetAllMaterials(string produit)
        {

            var articles = _SageDbContext.SageArticleDbo
                .Where(e => EF.Functions.Like(e.Reference, "MP%"))
                .ToList();


            var nomenclatures = _DevContext.nomenclatureDbo.ToList();


            var result = articles.Join(
                nomenclatures,
                article => article.Reference,
                nomenclature => nomenclature.ReferenceMP,
                (article, nomenclature) => new
                {
                    Designation = article.Designation,
                    produit = nomenclature.ReferencePf,
                    CodeFamille = article.codeFamille,
                    PrixTTC = article.PrixTTC,
                    Reference = article.Reference,
                    Quantity = nomenclature.Quantite,
                    Pourcentage = nomenclature.Pourcentage,
                    nomenclature = nomenclature.Id
                }).Where(e => e.produit == produit)
                .ToList<dynamic>();

            return result;
        }
        public async Task EditNomenclatures(List<Nomenclature> nomenclatures)
        {
            try
            {
                var nomenclaturesByProduct = nomenclatures.GroupBy(n => n.ReferencePf);

                foreach (var productGroup in nomenclaturesByProduct)
                {
                    string referencePf = productGroup.Key;
                    double totalPercentage = (double)productGroup.Sum(n => n.Pourcentage);

                    if (Math.Abs(totalPercentage - 100) > 0.001)
                    {
                        throw new Exception($"Total percentage for product {referencePf} must equal 100%. Current total: {totalPercentage}%");
                    }
                }

                foreach (var nomenclature in nomenclatures)
                {
                    var existingNomenclature = await _DevContext.nomenclatureDbo
                        .FirstOrDefaultAsync(n => n.Id == nomenclature.Id);
                Console.WriteLine(nomenclature.Pourcentage) ;
                    if (existingNomenclature != null)
                    {
                        existingNomenclature.ReferenceMP = nomenclature.ReferenceMP;
                        existingNomenclature.ReferencePf = nomenclature.ReferencePf;
                        existingNomenclature.Pourcentage = nomenclature.Pourcentage;
                        existingNomenclature.Quantite = nomenclature.Quantite;

                        _DevContext.nomenclatureDbo.Update(existingNomenclature);
                    }
                    else
                    {
                        await _DevContext.nomenclatureDbo.AddAsync(nomenclature);
                    }
                }
                await _DevContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error editing nomenclatures: {ex.Message}");
            }
        }
    }
}