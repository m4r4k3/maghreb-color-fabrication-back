using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fabrication_maghreb_color.application.repository
{
    public class FabricationRepository : IFabricationRepository
    {
        private readonly MainContext _dbContext;

        public FabricationRepository(MainContext context)
        {
            _dbContext = context;
        }

        public List<BonFabrication> GetAllBon()
        {
            return _dbContext.BonFabricationDbo
                .Include(b => b.files)
                .Include(b => b.matieres)
                .ThenInclude(m => m.Type)
                .ToList();
        }

        public List<PreparationFabrication> GetAllPreparation()
        {
            return _dbContext.PreparationFabricationsDbo
                .Include(e => e.Projet)
                    .ThenInclude(e => e.Nomenclatures)
                .Include(e => e.Projet)
                    .ThenInclude(p => p.Type)
                .Include(e => e.Bons)
                    .ThenInclude(b => b.files)
                .Include(e => e.Bons)
                    .ThenInclude(b => b.matieres)
                        .ThenInclude(m => m.Type)
                .ToList();
        }

        public async Task<PreparationFabrication> CreatePreparation(PreparationFabrication preparation)
        {
            _dbContext.PreparationFabricationsDbo.Add(preparation);
            await _dbContext.SaveChangesAsync();
            return preparation;
        }

        public async Task<BonFabrication> CreateBon(BonFabrication bon)
        {
            _dbContext.BonFabricationDbo.Add(bon);
            await _dbContext.SaveChangesAsync();
            return bon;
        }

        public async Task<BonFabrication> GetBonWithRelatedData(int bonId)
        {
            var bonWithRelated = await _dbContext.BonFabricationDbo
            .Include(b => b.files)
                .Include(b => b.preparationFabrication)
                    .ThenInclude(b => b.Projet)
                    .Include(e=>e.matieres)
                .Include(e=>e.files)
                .FirstOrDefaultAsync(b => b.Id == bonId);

            return bonWithRelated;
        }
        public async Task<PreparationFabrication> GetPreparationById(int Id)
        {
            var preparation = await _dbContext.PreparationFabricationsDbo.Include(e => e.Bons).Include(e => e.Projet).FirstOrDefaultAsync(e => e.Id == Id);

            return preparation;
        }

        public async Task StoreFile(BonFile bonFile)
        {
            _dbContext.BonFileDbo.Add(bonFile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateProjectHasNomenclature(int projectId)
        {
            var project = await _dbContext.Set<Projet>().FindAsync(projectId);
            if (project != null)
            {
                project.HasNomenclature = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task AddNomenclature(Nomenclature nomenclature)
        {
            _dbContext.NomenclatureDbo.Add(nomenclature);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateBon(BonFabrication bon)
        {
            if (bon == null)
            {
                throw new ArgumentNullException(nameof(bon));
            }

            _dbContext.BonFabricationDbo.Update(bon);
            await _dbContext.SaveChangesAsync();
        }
    }
}