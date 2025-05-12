using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{
    public class RolePoliciesRepository : IRolePoliciesRepository
    {
        private readonly MainContext _dbContext;

        public RolePoliciesRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task Create(Policies policies)
        {
            _dbContext.PoliciesDbo.Add(policies);
            await _dbContext.SaveChangesAsync();
        }
    }
}
