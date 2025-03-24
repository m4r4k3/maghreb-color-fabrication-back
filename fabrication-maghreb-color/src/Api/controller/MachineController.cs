using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.application.service;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        public readonly MachineService _service;
        public MachineController(MachineService service)
        {
            _service = service;
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
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}