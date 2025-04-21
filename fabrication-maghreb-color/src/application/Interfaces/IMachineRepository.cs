using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{

    public interface IMachineRepository
    {
        List<ChampLibre> GetAllMachines();
        Task<ChampLibre> MachineById(int? id);
    }
}
