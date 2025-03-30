using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.application.service;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompteController : ControllerBase
    {
        public readonly CompteService _service;
        ILogger<CompteController> _logger;
        public CompteController(CompteService service, ILogger<CompteController> logger)
        {
            _service = service;
        }

        [HttpGet("clients")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_service.getAllClients());
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());

                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}