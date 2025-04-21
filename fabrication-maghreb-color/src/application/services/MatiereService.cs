using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.Extensions.Logging;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{
    public class MatiereService
    {
        private readonly IMatiereRepository _repository;
        private readonly ILogger<MatiereService> _logger;

        public MatiereService(IMatiereRepository repository, ILogger<MatiereService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void Creation(Matiere matiere)
        {
            try
            {
                _repository.Create(matiere);
                _logger.LogInformation("Matiere created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating Matiere: {ex.Message}");
            }
        }
    }
}
