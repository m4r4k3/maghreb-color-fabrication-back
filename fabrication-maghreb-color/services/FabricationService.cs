using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.service
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
            return _dbContext.PreparationFabricationsDbo.Include(p => p.Projet)  // Eager load the related Projet
     .ToList();
        }
    }
}