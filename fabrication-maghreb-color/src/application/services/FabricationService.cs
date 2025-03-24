using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{
 
    public class PreparationFabricationService
    {
        private readonly MainContext _dbContext;
        public PreparationFabricationService(MainContext context)
        {
            _dbContext = context;
        }
        public List<PreparationFabrication> GetAllPreparation()
        {
return _dbContext.PreparationFabricationsDbo.Include(e=>e.Projet).Include(e=>e.Bons)
    .ToList();

        }
       public async Task CreatePreparition(PreparationFabrication preparation)
{
   
        _dbContext.PreparationFabricationsDbo.Add(preparation);
        await _dbContext.SaveChangesAsync();
}
public async Task CreateBon(BonFabrication bon){
    _dbContext.BonFabricationDbo.Add(bon);
    await _dbContext.SaveChangesAsync();
}
    }
}