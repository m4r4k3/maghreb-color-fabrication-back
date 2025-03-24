using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.service;

namespace fabrication_maghreb_color.api.controller
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class ArticleController : ControllerBase
    {
        public readonly ArticleService _service;
        public ArticleController(ArticleService projet)
        {
            _service = projet;
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
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }


    }
}