namespace fabrication_maghreb_color.Api.controller
{
    using fabrication_maghreb_color.application.Interfaces;
    using fabrication_maghreb_color.Infrastructure.model;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class CommandeController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        private readonly ILogger<CommandeController> _logger;
        public CommandeController(IDocumentService documentService, ILogger<CommandeController> logger)
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet]
        public ActionResult<List<Document>> GetAllCommandes()
        {
            try
            {
                var documents = _documentService.GetAllDocumentsByType(1);
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