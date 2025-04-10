using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.application.service;
using fabrication_maghreb_color.Infrastructure.model;
using System.Text.Json;
using fabrication_maghreb_color.Infrastructure.dto;
using fabrication_maghreb_color.Config.Sage;

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

        [HttpGet("preparation")]
        public IActionResult GetPrepartion()
        {
            try
            {
                return Ok(_service.GetAllPreparation());
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        [HttpPost("preparation")]
        public async Task<IActionResult> CreatePrepartion([FromBody] PreparationRequest requestData)
        {
            try
            {

                Decimal quantite = requestData.Quantite;
                dynamic document = requestData.Document;
                await _service.CreatePreparition(document);

                var updateData = new Dictionary<string, object>
                {
                    { "quantite", quantite }
                };
                await _serviceProjet.updateProjet(document.Projet_Id, updateData);

                return Ok(new
                {
                    message = "Prepartion créer"
                });
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        [HttpPost("bon")]
        public async Task<IActionResult> CreateBon([FromBody] BonRequest request) {
            try {
                BonFabrication bon = request.bon;
                bon.DateCreation = DateTime.Now;

                List<Matiere> matieres = await _service.CreateBon(bon, request.matieres);
                foreach (Matiere matiere in matieres)
                {
                    matiere.Bon_id = bon.Id;
                    matiere.DateAffection = DateTime.Now;
                    _serviceMatiere.creation(matiere);
                }

               
                return Ok(new
                {
                    message = "Prepartion créer"
                });
            } catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }


    }
}