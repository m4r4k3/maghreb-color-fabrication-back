using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.application.service;
using fabrication_maghreb_color.Infrastructure.model;
using System.Text.Json;
using FabricationMaghrebColor.Infrastructure.DTO;

namespace FabricationMaghrebColor.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FabricationController : ControllerBase
    {
        public readonly PreparationFabricationService _service;
        public readonly ProjetService _serviceProjet;
        public readonly MatiereService _serviceMatiere;
        public FabricationController(PreparationFabricationService service, ProjetService serviceProjet, MatiereService serviceMatiere)
        {
            _service = service;
            _serviceProjet = serviceProjet;
            _serviceMatiere = serviceMatiere;
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
                Console.WriteLine(err);
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
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        [HttpPost("bon")]
        public async Task<IActionResult> CreateBon([FromBody] BonRequest requestData)
        {
            try
            {
                BonFabrication bon = requestData.bon;
                bon.DateCreation = DateTime.Now;
                await _service.CreateBon(bon);    
                foreach(Matiere matiere in requestData.matieres)
                {
                    matiere.Bon_id = bon.Id;
                     _serviceMatiere.creation(matiere);
                }
                return Ok(new
                {
                    message = "Prepartion créer"
                });
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }

    }
}