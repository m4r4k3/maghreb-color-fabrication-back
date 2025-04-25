using fabrication_maghreb_color.Infrastructure.Repositories;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Sage;

namespace fabrication_maghreb_color.Application.Services
{
    public class ProjetService
    {
        private readonly IProjetRepository _projetRepository;
        private readonly IChargeCompteRepository _chargeCompteRepository;
        private readonly ICompteRepository _compteRepository;
        private readonly SageOM _sageOM;
        private readonly ILogger<ProjetService> _logger;
        public readonly IConfiguration? _configuration;

        public ProjetService(IProjetRepository projetRepository, ILogger<ProjetService> logger, IConfiguration? configuration, SageOM sageOM, IChargeCompteRepository chargeCompteRepository, ICompteRepository compteRepository)
        {
            _chargeCompteRepository = chargeCompteRepository;
            _compteRepository = compteRepository;
            _projetRepository = projetRepository;
            _configuration = configuration;
            _sageOM = sageOM;
            _logger = logger;
        }

        public async Task<bool> Create(Projet projet, IFormFile descriptionFile)
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
            Compte client = await _compteRepository.GetClientById(projet.ReferenceClient);

            string intitule = type.Abrege + " " + client.intitule + " " + projet.quantite;
            Dictionary<string, string> SageProperties =await _sageOM.CreateArticle(intitule, type.Reference, projet, _chargeCompteRepository.getById(projet.chargeCompteId));
            projet.ReferenceArticle = SageProperties["Reference"];
            projet.NumeroBC = SageProperties["NumeroBC"];
            _projetRepository.Add(projet);
            return true;
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
            _sageOM.UpdateBCQuantity(projet.NumeroBC, Convert.ToDouble(updateValues["quantite"]));
            if (projet == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            try
            {
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

                _projetRepository.Update(projet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project with ID: {ProjectId}", id);
                throw;
            }
        }
    }
}