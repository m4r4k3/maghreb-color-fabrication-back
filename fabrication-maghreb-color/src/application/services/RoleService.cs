using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.Extensions.Logging;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class RoleService
    {
        private readonly IRolesRepository _repository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRolesRepository repository, ILogger<RoleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public List<Role> GetAll()
        {

            return _repository.GetAllRoles();
            _logger.LogInformation("Matiere created successfully.");

        }
        public string Create(Role role)
        {

            _repository.Create(role);
            return "created";
        }
        public string Delete(int id)
        {
            Role role = _repository.GetById(id);
            _repository.Delete(role);
            return "deleted ";
        }

    }
}
