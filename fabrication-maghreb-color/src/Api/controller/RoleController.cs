using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        public readonly RoleService _service;
        public readonly ILogger<RoleController> _logger;

        public RoleController(RoleService service, ILogger<RoleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des roles.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des roles. Veuillez réessayer plus tard." });
            }
        }
    }
}
