using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class PermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public List<Policies> GetAllPermissions()
        {
            return _repository.GetAllPermissions();
        }
    }
}
