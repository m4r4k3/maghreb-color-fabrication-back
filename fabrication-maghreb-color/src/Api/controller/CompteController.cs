using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;
using fabrication_maghreb_color.Infrastructure.model;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompteController : ControllerBase
    {
        public readonly CompteService _service;
        private readonly ILogger<CompteController> _logger;

        public CompteController(CompteService service, ILogger<CompteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("clients")]
        public IActionResult Get()
        {
            try
            {
                var clients = _service.GetAllClients();
                return Ok(clients);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des clients.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des clients. Veuillez réessayer plus tard." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Compte compte)
        {
            try
            {
                await _service.CreateClient(compte);
                return Ok(new { message = "Client créé avec succès." });
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la création du client.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la création du client. Veuillez réessayer plus tard." });
            }
        }
    }
}
