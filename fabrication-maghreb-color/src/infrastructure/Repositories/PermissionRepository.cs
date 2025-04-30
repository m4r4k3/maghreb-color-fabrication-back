using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{

    public class PermissionRepository : IPermissionRepository
    {
        private readonly MainContext _context;

        public PermissionRepository(MainContext context)
        {
            _context = context;
        }

        public List<Policies> GetAllPermissions()
        {


            return _context.PoliciesDbo.Where(p => p.BelongsTo == null)
    .Include(p => p.Children).ToList();
        }

    }
}
