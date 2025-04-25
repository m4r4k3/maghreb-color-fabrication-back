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
    public class ArticleController : ControllerBase
    {
        public readonly ArticleService _service;
        public readonly ILogger<ArticleController> _logger;

        public ArticleController(ArticleService service, ILogger<ArticleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetArticleByFamily(string filter)
        {
            try
            {
                var articles = _service.GetFilteredArticles(filter);
                return Ok(articles);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Une erreur est survenue lors de la récupération des articles.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des articles. Veuillez réessayer plus tard." });
            }
        }
    }
}
