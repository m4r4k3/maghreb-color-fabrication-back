using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace fabrication_maghreb_color.application.service
{

    public class PreparationFabricationService
    {
        private readonly MainContext _dbContext;
        public readonly SageOM _SageOm;

        public PreparationFabricationService(MainContext context, SageOM sageOm)
        {
            _dbContext = context;
            _SageOm = sageOm;

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
        public async Task CreatePreparition(PreparationFabrication preparation)
        {

            _dbContext.PreparationFabricationsDbo.Add(preparation);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Matiere>> CreateBon(BonFabrication bon, List<Matiere> matieres)
        {
            // Add the bon and save
            _dbContext.BonFabricationDbo.Add(bon);
            await _dbContext.SaveChangesAsync();

            // Fetch related data
            var bonWithRelated = await _dbContext.BonFabricationDbo
                .Include(b => b.preparationFabrication)
                    .ThenInclude(b => b.Projet)
                .FirstOrDefaultAsync(b => b.Id == bon.Id);

            if (bonWithRelated == null)
            {
                throw new InvalidOperationException($"Failed to retrieve newly created bon with ID {bon.Id}");
            }

            // Create support material
            Matiere support = new Matiere
            {
                ReferenceMP = bonWithRelated.preparationFabrication.ReferenceArticleSup,
                TypeId = 3,
                DateAffection = DateTime.Now,
                QuantiteUtilise = (int)bonWithRelated.QuantiteSupport,
                Pourcentage = 100,
            };


            matieres.Add(support);

            // Get the reference client cleanly
            var projet = await _dbContext.ProjetDbo
                .Include(e => e.preparationFabrication)
                .FirstOrDefaultAsync(e => e.preparationFabrication.Id == bonWithRelated.Pf_id);

            string referenceClient = projet?.ReferenceClient ?? string.Empty;

            if (!projet.HasNomenclature)
            {
                await _SageOm.CreateNomenclature(projet.ReferenceArticle, matieres);
                foreach (Matiere matiere in matieres)
                {
                    _dbContext.NomenclatureDbo.Add(new Nomenclature
                    {
                        ReferenceMP = matiere.ReferenceMP,
                        ProjetId = projet.Id,
                        Type = matiere.TypeId,
                    });
                    _dbContext.SaveChanges();
                }
                projet.HasNomenclature = true;
                await _dbContext.SaveChangesAsync();
            }


            // Capture the result instead of writing to console
            var result = _SageOm.CreeBonFabrication(bonWithRelated, referenceClient, matieres);

            // Log or handle the result appropriately
            // logger.Log(result);

            return matieres;
        }
        public async Task StoreFile(BonFile bonFile)
        {
            _dbContext.BonFileDbo.Add(bonFile);
            await _dbContext.SaveChangesAsync();
        }

    }
}