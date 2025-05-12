using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;
using fabrication_maghreb_color.Infrastructure.model;
using System.Text.Json;
using fabrication_maghreb_color.Infrastructure.dto;
using fabrication_maghreb_color.Config.Sage;
using Microsoft.AspNetCore.Authorization;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricationController : ControllerBase
    {
        public readonly PreparationFabricationService _service;
        public readonly ProjetService _serviceProjet;
        public readonly MatiereService _serviceMatiere;
        public readonly ILogger<FabricationController> _logger;

        public FabricationController(PreparationFabricationService service, ProjetService serviceProjet, MatiereService serviceMatiere, ILogger<FabricationController> logger)
        {
            _service = service;
            _serviceProjet = serviceProjet;
            _serviceMatiere = serviceMatiere;
            _logger = logger;
        }

        [Authorize("Valider Fin Fabrication")]
        [HttpGet("preparation")]
        public IActionResult GetPrepartion()
        {
            try
            {
                var preparations = _service.GetAllPreparation();
                return Ok(preparations);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des préparations.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des préparations. Veuillez réessayer plus tard." });
            }
        }

        [Authorize("Voir Bons Fabrication")]
        [HttpGet("bon")]
        public IActionResult GetBon()
        {
            try
            {
                var bons = _service.GetAllBon();
                return Ok(bons);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des bons.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des bons. Veuillez réessayer plus tard." });
            }
        }

        [Authorize("Préparer Projet")]
        [HttpPost("preparation")]
        public async Task<IActionResult> CreatePrepartion([FromBody] PreparationRequest requestData)
        {
            try
            {
                Decimal quantite = requestData.Quantite;
                dynamic document = requestData.Document;

                await _service.CreatePreparation(document);

                var updateData = new Dictionary<string, object>
                {
                    { "quantite", quantite }
                };
                await _serviceProjet.UpdateProjet(document.Projet_Id, updateData);

                return Ok(new { message = "Préparation créée avec succès." });
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la création de la préparation.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la création de la préparation. Veuillez réessayer plus tard." });
            }
        }

[Authorize("Créer Bon Fabrication")]
        [HttpPost("bon")]
        public async Task<IActionResult> CreateBon([FromForm] BonRequest request)
        {
            try
            {
                BonFabrication bon = request.bon;
                bon.DateCreation = DateTime.Now;

                List<Matiere> matieres = await _service.CreateBon(bon, request.matieres);
                foreach (Matiere matiere in matieres)
                {
                    matiere.Bon_id = bon.Id;
                    _serviceMatiere.Creation(matiere);
                }

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/bf");

                string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };

                foreach (IFormFile file in request.files)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (allowedImageExtensions.Contains(fileExtension))
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        var path = Path.Combine(folderPath, uniqueFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            await _service.StoreFile(new BonFile { BonId = bon.Id, FilePath = uniqueFileName });
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Le fichier {file.FileName} n'est pas une image valide.");
                    }
                }

                return Ok(new { message = "Bon créé avec succès." });
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la création du bon.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la création du bon. Veuillez réessayer plus tard." });
            }
        }

        [Authorize("Créer Bon Finition")]
        [HttpPost("finition")]
        public async Task<IActionResult> CreateFinition([FromBody] FinitionDto requestData)
        {
            try
            {
                await _service.Finir(requestData);

                return Ok(new { message = "Finition créée avec succès." });
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la création de la finition.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la création de la finition. Veuillez réessayer plus tard." });
            }
        }
    }
}
