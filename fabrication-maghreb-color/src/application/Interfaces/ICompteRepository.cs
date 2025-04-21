using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface ICompteRepository
    {
        List<Compte> GetAllClients();
        Task CreateClient(Compte compte);
    }
}