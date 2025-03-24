using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{

    public class MachineService
    {
        private readonly SageContext _dbContext;
        public MachineService(SageContext context)
        {
            _dbContext = context;
        }
        public List<ChampLibre> GetAllMachines()
        {
            return _dbContext.Machine.ToList();
        }
    }
}