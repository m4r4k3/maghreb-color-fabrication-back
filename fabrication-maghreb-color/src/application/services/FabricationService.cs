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
            return _dbContext.PreparationFabricationsDbo.Include(e => e.Projet).Include(e => e.Bons)
                .ToList();

        }
        public async Task CreatePreparition(PreparationFabrication preparation)
        {

            _dbContext.PreparationFabricationsDbo.Add(preparation);
            await _dbContext.SaveChangesAsync();
        }
        public async Task CreateBon(BonFabrication bon)
        {
            _dbContext.BonFabricationDbo.Add(bon);
            await _dbContext.SaveChangesAsync();
            _SageOm.CreeBonFabrication(bon,
                _dbContext.ProjetDbo
    .Include(e => e.preparationFabrication)
    .FirstOrDefault(e => e.preparationFabrication.Id == bon.Pf_id).ReferenceClient
);

        }
    }
}