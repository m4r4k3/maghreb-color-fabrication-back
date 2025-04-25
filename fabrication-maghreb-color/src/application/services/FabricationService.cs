using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Infrastructure.dto;

namespace fabrication_maghreb_color.Application.Services
{


    public class PreparationFabricationService : IPreparationFabricationService
    {
        private readonly IFabricationRepository _repository;
        private readonly IMachineRepository _machineRepository;
        private readonly IChargeCompteRepository _chargeCompteRepository;
        private readonly SageOM _sageOm;

        public PreparationFabricationService(
            IFabricationRepository repository,
            SageOM sageOm,
            IMachineRepository machineRepository,
            IChargeCompteRepository chargeCompteRepository)
            {
            _repository = repository;
            _machineRepository = machineRepository;
            _sageOm = sageOm;
            _chargeCompteRepository = chargeCompteRepository;
            }

        public List<BonFabrication> GetAllBon()
        {
            return _repository.GetAllBon();
        }

        public List<PreparationFabrication> GetAllPreparation()
        {
            return _repository.GetAllPreparation();
        }

        public async Task CreatePreparation(PreparationFabrication preparation)
        {
            await _repository.CreatePreparation(preparation);
        }

        public async Task<List<Matiere>> CreateBon(BonFabrication bon, List<Matiere> matieres)
        {
            PreparationFabrication preparation = await _repository.GetPreparationById(bon.Pf_id);
            if (preparation.Projet.quantite - preparation.quantiteConsomme - bon.quantite < 0)
            {
                throw new InvalidOperationException($"Insufficient quantity available for preparation with ID {bon.Pf_id}.");
            }
            // Create the bon
            await _repository.CreateBon(bon);

            // Fetch related data
            var bonWithRelated = await _repository.GetBonWithRelatedData(bon.Id);

            if (bonWithRelated == null)
            {
                throw new InvalidOperationException($"Failed to retrieve newly created bon with ID {bon.Id}");
            }

            // Create support material
            matieres.Add(new Matiere
            {
                ReferenceMP = bonWithRelated.preparationFabrication.ReferenceArticleSup,
                TypeId = 3,
                DateAffection = DateTime.Now,
                QuantiteUtilise = (int)bonWithRelated.QuantiteSupport,
                Pourcentage = 100,
            });

            Projet projet = bonWithRelated.preparationFabrication.Projet;

            if (!projet.HasNomenclature)
            {
                // Create nomenclature in Sage
                await _sageOm.CreateNomenclature(projet.ReferenceArticle, matieres);

                // Add nomenclature entries to database
                foreach (Matiere matiere in matieres)
                {
                    await _repository.AddNomenclature(new Nomenclature
                    {
                        ReferenceMP = matiere.ReferenceMP,
                        ProjetId = projet.Id,
                        Type = matiere.TypeId,
                    });
                }

                // Update project's HasNomenclature flag
                await _repository.UpdateProjectHasNomenclature(projet.Id);
            }

            // Return the updated list of materials
            return matieres;
        }

        public async Task StoreFile(BonFile bonFile)
        {
            await _repository.StoreFile(bonFile);
        }
        public async Task Finir(FinitionDto finition)
        {
            var bon = await _repository.GetBonWithRelatedData(finition.BonId);
            if (bon == null)
            {
                throw new InvalidOperationException($"Bon with ID {finition.BonId} not found.");
            }
            if (bon.Fini)
            {
                throw new InvalidOperationException($"Bon with ID {finition.BonId} is already finished.");
            }

            bon.Fini = true;
            bon.QuantiteFini = finition.QuantiteFini;
            bon.NombreBobins = finition.NombreBobins;

            await _repository.UpdateBon(bon);
            bon.machine = await _machineRepository.MachineById(bon.MachineId);
            int ChargeId = bon.preparationFabrication.Projet.chargeCompteId;
            Console.WriteLine("chargeId" + ChargeId);

            chargeCompte charge = _chargeCompteRepository.getById(ChargeId);

            await _sageOm.CreeBonFabrication(bon, charge);

        }
    }
}