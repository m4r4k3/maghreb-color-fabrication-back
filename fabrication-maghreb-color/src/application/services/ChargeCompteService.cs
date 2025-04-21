using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.repository;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class ChargeCompteService
    {
        private readonly IChargeCompteRepository _repo;

        public ChargeCompteService(IChargeCompteRepository repository)
        {
            _repo = repository;
        }

        public List<chargeCompte> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
