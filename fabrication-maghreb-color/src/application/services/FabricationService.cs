using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace fabrication_maghreb_color.application.service
{

    public class PreparationFabricationService
    {
        private readonly MainContext _dbContext;
        public readonly SageOM _SageOm;

        public PreparationFabricationService(MainContext context, SageOM sageOm)
        {
            _dbContext = context;
            _SageOm = sageOm;

        }
        public List<PreparationFabrication> GetAllPreparation()
        {
            return _dbContext.PreparationFabricationsDbo.Include(e => e.Projet).Include(e => e.Projet.Type).Include(e => e.Bons).ThenInclude(e => e.matieres).ThenInclude(e => e.Type)
                .ToList();

        }
        public async Task CreatePreparition(PreparationFabrication preparation)
        {

            _dbContext.PreparationFabricationsDbo.Add(preparation);
            await _dbContext.SaveChangesAsync();
        }
      public async Task<List<Matiere>> CreateBon(BonFabrication bon, List<Matiere> matieres)
        {
            _dbContext.BonFabricationDbo.Add(bon);
            await _dbContext.SaveChangesAsync();

            var bonWithRelated = await _dbContext.BonFabricationDbo
    .Include(b => b.preparationFabrication) // Replace 'RelatedEntity' with the actual navigation property
    .FirstOrDefaultAsync(b => b.Id == bon.Id);
            Matiere support = new Matiere
            {
                ReferenceMP = bon.preparationFabrication.ReferenceArticleSup,
                TypeId = 3,
                DateAffection = DateTime.Now,
                QuantiteUtilise = (int)bon.QuantiteSupport,
                Pourcentage = 100,
            };

            matieres.Add(support);
            Console.WriteLine(_SageOm.CreeBonFabrication(
                 bon,
               _dbContext.ProjetDbo
                     .Include(e => e.preparationFabrication)
                     .FirstOrDefault(e => e.preparationFabrication.Id == bon.Pf_id)?
                     .ReferenceClient

            , matieres));
            return matieres;
        }


    }
}