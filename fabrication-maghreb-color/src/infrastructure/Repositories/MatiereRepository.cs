using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{
    
    public class MatiereRepository :IMatiereRepository 
    {
        private readonly MainContext _context;

        public MatiereRepository(MainContext context)
        {
            _context = context;
        }

        public void Create(Matiere matiere)
        {
            _context.MatiereDbo.Add(matiere);
            _context.SaveChanges();
        }
    }
}
