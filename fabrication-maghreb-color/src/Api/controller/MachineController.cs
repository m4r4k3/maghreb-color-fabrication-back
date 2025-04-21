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
                return Ok(_service.GetAllMachines());
            }
            catch (Exception err)
            {
                              _logger.LogError(err.ToString());

                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}