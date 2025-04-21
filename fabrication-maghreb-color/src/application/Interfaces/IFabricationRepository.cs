using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
   public interface IFabricationRepository
    {
        List<BonFabrication> GetAllBon();
        List<PreparationFabrication> GetAllPreparation();
        Task<PreparationFabrication> CreatePreparation(PreparationFabrication preparation);
        Task<BonFabrication> CreateBon(BonFabrication bon);
        Task<BonFabrication> GetBonWithRelatedData(int bonId);
        Task StoreFile(BonFile bonFile);
        Task<bool> UpdateProjectHasNomenclature(int projectId);
         Task<PreparationFabrication> GetPreparationById(int Id) ;
         Task UpdateBon(BonFabrication bon) ;
        Task<int> SaveChangesAsync();
        Task AddNomenclature(Nomenclature nomenclature);
    }

}