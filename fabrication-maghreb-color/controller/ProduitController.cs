using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.model;
using fabrication_maghreb_color.service;

namespace fabrication_maghreb_color.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProduitController : ControllerBase
    {
        public readonly ProduitService _service;
        public ProduitController(ProduitService projet)
        {
            _service = projet;
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
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        
    }
}
