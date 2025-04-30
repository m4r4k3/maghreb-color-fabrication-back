namespace fabrication_maghreb_color.Api.controller
{
    using fabrication_maghreb_color.application.Interfaces;
    using fabrication_maghreb_color.Infrastructure.dto;
    using fabrication_maghreb_color.Infrastructure.model;
    using Microsoft.AspNetCore.Authorization;
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

        [Authorize("SeeBonCommande")]
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
                _logger.LogError(ex, "Une erreur est survenue lors de la récupération des commandes.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la récupération des commandes. Veuillez réessayer plus tard." });
            }
        }

        [Authorize("TransferBonCommande")]
        [HttpPost("transform")]
        public async Task<ActionResult> TransformDocument([FromBody] DocumentDto document)
        {
            try
            {
                await _documentService.TransformDocument(document);
                return Ok(new { message = "Document transformé avec succès." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de la transformation du document.");
                return StatusCode(500, new { status = "error", message = "Une erreur interne est survenue lors de la transformation du document. Veuillez réessayer plus tard." });
            }
        }
    }
}
