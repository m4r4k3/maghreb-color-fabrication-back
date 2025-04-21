using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{
    public class MachineRepository : IMachineRepository
    {
        private readonly SageContext _dbContext;

        public MachineRepository(SageContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ChampLibre> GetAllMachines()
        {
            return _dbContext.Machine.ToList();
        }
        public async Task<ChampLibre> MachineById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return _dbContext.Machine.FirstOrDefault(m => m.Id == id);
        }
    }
}
