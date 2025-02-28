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
    public class ArticleController : ControllerBase
    {
        public readonly ArticleService _service;
        public ArticleController(ArticleService projet)
        {
            _service = projet;
        }
        [HttpGet("produits")]
        public IActionResult GetProduits()
        {
            try
            {
                return Ok(_service.GetAllProducts());
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        [HttpGet("materiel")]
        public IActionResult GetMaterial([FromQuery] string? produit)
        {
            try
            {
                if (produit == null)
                {
                    return Ok(_service.GetAllMaterialsByProduct());
                }
                else
                {
                    return Ok(_service.GetAllMaterials(produit));
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }
        [HttpPost("nomenclature")]
        public async Task<IActionResult> CreateNomenclature([FromBody] List<Nomenclature> nomenclatures)
        {
            try
            {
                await _service.EditNomenclatures(nomenclatures);
                return Ok(new { message = "success" });
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(new { status = "error", message = "Error occured" });
            }
        }

    }
}
