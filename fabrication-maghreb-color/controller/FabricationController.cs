using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.service;

namespace FabricationMaghrebColor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricationController : ControllerBase
    {
        public readonly PreparationFabricationService _service;
        public FabricationController(PreparationFabricationService service)
        {
            _service = service;
        }

        [HttpGet("preparation")]
        public IActionResult Get()
        {
             try
            {
                return Ok(_service.GetAllPreparation());
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}