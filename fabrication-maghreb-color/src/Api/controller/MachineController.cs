using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        public readonly MachineService _service;
        public readonly ILogger<MachineController> _logger;

        public MachineController(MachineService service, ILogger<MachineController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var machines = _service.GetAllMachines();
               
                return Ok(machines);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des machines.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des machines. Veuillez réessayer plus tard." });
            }
        }
    }
}
