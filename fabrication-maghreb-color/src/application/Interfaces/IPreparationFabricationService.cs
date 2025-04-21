using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IPreparationFabricationService
    {
        List<BonFabrication> GetAllBon();
        List<PreparationFabrication> GetAllPreparation();
        Task CreatePreparation(PreparationFabrication preparation);
        Task<List<Matiere>> CreateBon(BonFabrication bon, List<Matiere> matieres);
        Task StoreFile(BonFile bonFile);
    }
}