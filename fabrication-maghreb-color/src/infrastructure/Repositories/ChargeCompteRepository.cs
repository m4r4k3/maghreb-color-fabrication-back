using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{
    public class ChargeCompteRepository : IChargeCompteRepository
    {
        private readonly MainContext _dbContext;

        public ChargeCompteRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<chargeCompte> GetAll()
        {
            return _dbContext.ChargeCompteDbo.ToList();
        }
    }
}
