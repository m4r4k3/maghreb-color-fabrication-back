using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class CompteService
    {
        private readonly ICompteRepository _repository;

        public CompteService(ICompteRepository repository)
        {
            _repository = repository;
        }

        public List<Compte> GetAllClients()
        {
            return _repository.GetAllClients();
        }

        public async Task CreateClient(Compte compte)
        {
            await _repository.CreateClient(compte);
        }
    }
}
