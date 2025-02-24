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
    public class ProjetController : ControllerBase
    {
        public readonly ProjetService _service;
        public ProjetController(ProjetService projet)
        {
            _service = projet;
        }
        [HttpPost("create")]
        public IActionResult create([FromBody] Projet projet)
        {

            if (_service.create(projet))
            {
                return Ok(new
                {
                    status = "success",
                    message = "Projet created",
                });

            }
            else
            {
                return BadRequest(new { status = "error", message = "Projet not created" });
            }
        }
    }
}
