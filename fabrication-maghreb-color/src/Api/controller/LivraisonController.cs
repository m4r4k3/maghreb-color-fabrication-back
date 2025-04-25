namespace fabrication_maghreb_color.Api.controller
{
    using fabrication_maghreb_color.application.Interfaces;
    using fabrication_maghreb_color.Infrastructure.model;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class LivraisonController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        private readonly ILogger<LivraisonController> _logger;
        public LivraisonController(IDocumentService documentService, ILogger<LivraisonController> logger)
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet]
        public ActionResult<List<Document>> GetAllLivraisons()
        {
            try
            {
                var documents = _documentService.GetAllDocumentsByType(3);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching documents.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}