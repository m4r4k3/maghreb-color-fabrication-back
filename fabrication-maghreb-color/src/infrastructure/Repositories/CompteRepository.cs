using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{


    public class CompteRepository : ICompteRepository
    {
        private readonly SageContext _dbContext;
        private readonly SageOM _sageOM;

        public CompteRepository(SageContext dbContext, SageOM sageOM)
        {
            _dbContext = dbContext;
            _sageOM = sageOM;
        }

        public List<Compte> GetAllClients()
        {
            return _dbContext.ComptetDbo.Where(e => e.type == 0).ToList();
        }

        public async Task<Compte> GetClientById(string reference)
        {
            return await _dbContext.ComptetDbo
                   .Where(e => e.type == 0 && e.num.Equals(reference))
                   .FirstOrDefaultAsync();
        }


        public async Task CreateClient(Compte compte)
        {
            // Assuming CreateClient is async-compatible; otherwise, remove async/await
            _sageOM.CreateClient(compte);
            await Task.CompletedTask;
        }
    }
}
