using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class DetailsService : IDetailsService
    {
        private readonly IDetailsRepository _repository;

        public DetailsService(IDetailsRepository repository)
        {
            _repository = repository;
        }

        public async Task InsertDetailsOffset(InformationOffset details)
        {
            await _repository.InsertDetailsOffset(details);
        }
    }
}
