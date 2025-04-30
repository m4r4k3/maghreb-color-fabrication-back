using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;
using fabrication_maghreb_color.Infrastructure.Repositories;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        public readonly PermissionRepository _service;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(PermissionRepository service, ILogger<PermissionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var chargeComptes = _service.GetAllPermissions();
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
