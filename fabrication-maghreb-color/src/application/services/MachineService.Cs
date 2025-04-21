using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class MachineService
    {
        private readonly IMachineRepository _repository;

        public MachineService(IMachineRepository repository)
        {
            _repository = repository;
        }

        public List<ChampLibre> GetAllMachines()
        {
            return _repository.GetAllMachines();
        }
    }
}
