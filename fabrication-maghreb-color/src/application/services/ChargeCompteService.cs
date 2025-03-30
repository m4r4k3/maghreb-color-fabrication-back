using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{

    public class ChargeCompteService
    {
        private readonly MainContext _dbContext;
        public ChargeCompteService(MainContext context)
        {
            _dbContext = context;
        }
        public List<chargeCompte> GetAll()
        {
            return _dbContext.ChargeCompteDbo.ToList();
        }
    }
}