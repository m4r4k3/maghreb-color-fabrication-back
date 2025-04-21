using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.Extensions.Logging;
using System.IO;
using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Sage;

namespace fabrication_maghreb_color.Application.Services
{
    public class ProjetService
    {
        private readonly IProjetRepository _projetRepository;
        private readonly SageOM _sageOM;
        private readonly ILogger<ProjetService> _logger;
        public readonly IConfiguration? _configuration;

        public ProjetService(IProjetRepository projetRepository, ILogger<ProjetService> logger, IConfiguration? configuration, SageOM sageOM)
        {
            _projetRepository = projetRepository;
            _configuration = configuration;
            _sageOM = sageOM;
            _logger = logger;
        }

        public async Task<bool> Create(Projet projet, IFormFile descriptionFile)
        {
            try
            {
                if (descriptionFile != null)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(descriptionFile.FileName)}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/projet", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await descriptionFile.CopyToAsync(stream);
                        projet.Description = "/uploads/projet/" + uniqueFileName;
                        projet.TypeDescription = 1;
                    }
                }

                TypeProjet type = _projetRepository.GetTypeById(projet.TypeProjet);
                string intitule = type.Abrege + " " + projet.ReferenceClient + " " + projet.quantite;
                projet.ReferenceArticle = _sageOM.CreateArticle(intitule, type.Reference ,projet );

                _projetRepository.Add(projet);
                _projetRepository.SaveChanges();
                return true;
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return false;
            }
        }

        public List<TypeProjet> GetAllTypes()
        {
            return _projetRepository.GetAllTypes();
        }

        public List<Projet> GetAll()
        {
            return _projetRepository.GetAll();
        }

        public async Task UpdateProjet(int? id, Dictionary<string, object> updateValues)
        {
            var projet = await _projetRepository.GetById(id ?? 0);

            if (projet == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            foreach (var property in updateValues)
            {
                var propertyName = property.Key;
                var propertyValue = property.Value?.ToString();

                var projectProperty = projet.GetType().GetProperty(propertyName);
                if (projectProperty != null && projectProperty.CanWrite)
                {
                    if (Nullable.GetUnderlyingType(projectProperty.PropertyType) != null)
                    {
                        if (string.IsNullOrEmpty(propertyValue))
                        {
                            projectProperty.SetValue(projet, null);
                        }
                        else
                        {
                            var underlyingType = Nullable.GetUnderlyingType(projectProperty.PropertyType);
                            var convertedValue = Convert.ChangeType(propertyValue, underlyingType);
                            projectProperty.SetValue(projet, convertedValue);
                        }
                    }
                    else
                    {
                        projectProperty.SetValue(projet, Convert.ChangeType(propertyValue, projectProperty.PropertyType));
                    }
                }
            }

            _projetRepository.SaveChanges();
        }
    }
}
