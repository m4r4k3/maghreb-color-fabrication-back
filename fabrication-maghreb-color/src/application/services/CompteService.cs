using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.service
{
    public class CompteService
    {
        private readonly SageContext _dbContext;
        public CompteService(SageContext context)
        {
            _dbContext = context;
        }
        public List<Compte> getAllClients(){
            return _dbContext.ComptetDbo.Where(e=>e.type == 0).ToList( );
        }
    }
}
