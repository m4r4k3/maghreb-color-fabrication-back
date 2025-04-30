using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IPermissionRepository
    {
       public List<Policies> GetAllPermissions();
    }
}