using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChargeCompteController : ControllerBase
    {
        public readonly ChargeCompteService _service;
        private readonly ILogger<ChargeCompteController> _logger;

        public ChargeCompteController(ChargeCompteService service, ILogger<ChargeCompteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var chargeComptes = _service.GetAll();
                return Ok(chargeComptes);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des charges de compte.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des charges de compte. Veuillez réessayer plus tard." });
            }
        }
    }
}
