using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Application.Services;
using fabrication_maghreb_color.Application.Services;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChargeCompteController : ControllerBase
    {
        public readonly ChargeCompteService _service;
        public readonly ILogger<ChargeCompteController> _logger;

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
                return Ok(_service.GetAll());
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());

                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}