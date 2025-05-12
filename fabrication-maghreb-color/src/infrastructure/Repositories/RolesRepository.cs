using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly MainContext _dbContext;

        public RolesRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role GetRolesWithInclusion(string role)
        {
            return _dbContext.RoleDbo.Include(e => e.rolePolicies).ThenInclude(e => e.Policies).FirstOrDefault(e => e.Label == role);
        }
        public List<Role> GetAllRoles()
        {
            return _dbContext.RoleDbo.Include(e => e.rolePolicies).ThenInclude(e => e.Policies).ToList();
        }
        public async Task Create(Role role)
        {
            _dbContext.RoleDbo.Add(role);
            await _dbContext.SaveChangesAsync();
        }
        public Role GetById(int id)
        {
            return _dbContext.RoleDbo.Include(e => e.rolePolicies).FirstOrDefault(e => e.Id.Equals(id));
        }
        public async Task Delete(Role role)
        {
            _dbContext.RoleDbo.Remove(role);
            await _dbContext.SaveChangesAsync();

        }
    }
}
