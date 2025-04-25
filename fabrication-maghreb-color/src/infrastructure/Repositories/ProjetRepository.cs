using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;

namespace fabrication_maghreb_color.Infrastructure.Repositories
{
    public class ProjetRepository : IProjetRepository
    {
        private readonly MainContext _context;

        public ProjetRepository(MainContext context)
        {
            _context = context;
        }

        public Task<Projet> GetById(int id)
        {
            return _context.ProjetDbo.Include(p => p.preparationFabrication)
                                      .Include(p => p.Type)
                                      .FirstOrDefaultAsync(p => p.Id == id);
        }

        public List<TypeProjet> GetAllTypes()
        {
            return _context.TypeProjetDbo.ToList();
        }

        public List<Projet> GetAll()
        {
            return _context.ProjetDbo.Include(p => p.preparationFabrication)
                                      .Include(p => p.Type)
                                      .ToList();
        }

        public void Add(Projet projet)
        {
            _context.ProjetDbo.Add(projet);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public TypeProjet GetTypeById(int typeProjetId)
        {
            return _context.TypeProjetDbo.Find(typeProjetId);
        }
    }
}
