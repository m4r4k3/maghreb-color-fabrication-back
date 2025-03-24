using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{
    public class MatiereService
    {
        private readonly MainContext _context;
        private readonly ILogger<MatiereService> _logger;
        public MatiereService(MainContext context, ILogger<MatiereService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void creation(Matiere matiere)
        {
            _context.MatiereDbo.Add(matiere);
            _context.SaveChanges() ;
        }
    }
}