using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IRolesRepository
    {
        public Role GetRolesWithInclusion(string role) ;
        public List<Role> GetAllRoles() ;
    }
}