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
        public readonly ILogger<ArticleService> _logger;
        public ArticleController(ArticleService projet, ILogger<ArticleService> logger)
        {
            _service = projet;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetArticleByFamily(string filter)
        {
            try
            {
                return Ok(_service.GetFilteredArticles(filter));
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }


    }
}