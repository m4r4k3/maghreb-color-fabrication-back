using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IProjetRepository
    {
        Task<Projet> GetById(int id);
        List<TypeProjet> GetAllTypes();
        List<Projet> GetAll();
        void Add(Projet projet);
        void Update(Projet projet);

        TypeProjet GetTypeById(int typeProjetId);
    }
}