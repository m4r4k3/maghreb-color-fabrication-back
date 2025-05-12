using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.Extensions.Logging;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class RolePoliciesService
    {
        private readonly IRolePoliciesRepository _repository;
        private readonly ILogger<RolePoliciesService> _logger;

        public RolePoliciesService(IRolePoliciesRepository repository, ILogger<RolePoliciesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public void Create(Policies policies)
        {
            _repository.Create(policies);
        }

    }
}
