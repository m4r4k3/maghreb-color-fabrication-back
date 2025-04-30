using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.Application.Services;

namespace fabrication_maghreb_color.api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetController : ControllerBase
    {
        public readonly ProjetService _service;
        public readonly ILogger<ProjetController> _logger;

        public ProjetController(ProjetService projet, ILogger<ProjetController> logger)
        {
            _service = projet;
            _logger = logger;
        }
        [Authorize("Voir Projets")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var projects = _service.GetAll();
                return Ok(projects);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur interne est survenue lors de la récupération des projets. Veuillez réessayer plus tard."
                });
            }
        }
        [Authorize("CreateProject")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Projet projet, [FromForm] IFormFile? descriptionFile)
        {
            try
            {
                bool isCreated = await _service.Create(projet, descriptionFile);
                if (isCreated)
                {
                    return Ok(new
                    {
                        status = "success",
                        message = "Le projet a été créé avec succès."
                    });
                }
                else
                {
                    return BadRequest(new { status = "error", message = "Échec de la création du projet. Veuillez vérifier les informations fournies." });
                }
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur interne est survenue lors de la création du projet. Veuillez réessayer plus tard."
                });
            }
        }

        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            try
            {
                var projectTypes = _service.GetAllTypes();

                return Ok(projectTypes);
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "error",
                    message = "Une erreur interne est survenue lors de la récupération des types de projet. Veuillez réessayer plus tard."
                });
            }
        }
    }
}
