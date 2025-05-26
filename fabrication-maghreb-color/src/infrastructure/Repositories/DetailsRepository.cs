using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{

    public class DetailsRepository : IDetailsRepository
    {
        private readonly MainContext _dbContext;

        public DetailsRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertDetailsOffset(InformationOffset details)
        {
            _dbContext.Add(details);
            await _dbContext.SaveChangesAsync();
        }

    }
}