using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.Application.Services;

namespace fabrication_maghreb_color.api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class ProjetController : ControllerBase
    {
        public readonly ProjetService _service;
        public readonly ILogger<ProjetController> _logger;

        public ProjetController(ProjetService projet, ILogger<ProjetController> logger)
        {
            _service = projet;
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
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Projet projet, [FromForm] IFormFile? descriptionFile)
        {




            if (await _service.Create(projet, descriptionFile))
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
        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            try
            {
                return Ok(_service.GetAllTypes());
            }
            catch (Exception err)
            {
                                _logger.LogError(err.ToString());

                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
    }
}
