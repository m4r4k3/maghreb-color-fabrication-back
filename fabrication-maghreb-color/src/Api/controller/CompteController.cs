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
        ILogger<CompteController> _logger;
        public CompteController(CompteService service, ILogger<CompteController> logger)
        {
            _service = service;
            _logger = logger ;
        }

        [HttpGet("clients")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_service.GetAllClients());
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());

                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Compte compte)
        {
            try
            {
                await _service.CreateClient(compte);
                return Ok();
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());

                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}