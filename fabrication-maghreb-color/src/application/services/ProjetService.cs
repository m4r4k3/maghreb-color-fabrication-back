using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Config.Sage;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{
    public class ProjetService
    {
        private readonly MainContext _dbContext;
        private readonly ILogger<ProjetService> _logger;
        public readonly IConfiguration? _configuration;
        public readonly SageOM _sageOM;
        public ProjetService(MainContext context, ILogger<ProjetService> logger, IConfiguration? configuration, SageOM sageOM)
        {
            _dbContext = context;
            _configuration = configuration;
            _sageOM = sageOM;
            _logger = logger;
        }
        public async Task<bool> create(Projet projet, IFormFile descriptionFile)
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
                        projet.Description =  "/uploads/projet/" + uniqueFileName  ;
                        projet.TypeDescription = 1;
                    }
                }
                TypeProjet type = _dbContext.TypeProjetDbo.Find(projet.TypeProjet);
                string intitule = type.Abrege + " " + projet.ReferenceClient + " " + projet.quantite;
                projet.ReferenceArticle = _sageOM.CreateArticle(intitule, type.Reference);
                _dbContext.ProjetDbo.Add(projet);
                _dbContext.SaveChanges();
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
            return _dbContext.TypeProjetDbo.ToList();
        }
        public List<Projet> GetAll(int? type)
        {
            if (type.HasValue)
            {
                return _dbContext.ProjetDbo
                    .Include(p => p.preparationFabrication)
                    .Include(p => p.Type)
                    .Include(p => p.ChargeCompte)
                    .Where(e => e.TypeProjet == type.Value)
                    .ToList();
            }
            else
            {
                return _dbContext.ProjetDbo.Include(p => p.preparationFabrication)
                    .Include(p => p.Type)
                    .Include(p => p.ChargeCompte).ToList();
            }
        }
        public async Task updateProjet(int? id, Dictionary<string, object> updateValues)
        {
            Projet projet = _dbContext.ProjetDbo.Include(p => p.preparationFabrication).FirstOrDefault(e => e.Id == id);

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

                    // Handle nullable types properly
                    if (Nullable.GetUnderlyingType(projectProperty.PropertyType) != null)
                    {
                        // If the property is nullable and value is null/empty, set it to null
                        if (string.IsNullOrEmpty(propertyValue))
                        {
                            projectProperty.SetValue(projet, null);
                        }
                        else
                        {
                            // Convert to the underlying type of the nullable
                            var underlyingType = Nullable.GetUnderlyingType(projectProperty.PropertyType);
                            var convertedValue = Convert.ChangeType(propertyValue, underlyingType);
                            projectProperty.SetValue(projet, convertedValue);
                        }
                    }
                    else
                    {
                        // Handle non-nullable types as before
                        projectProperty.SetValue(projet, Convert.ChangeType(propertyValue, projectProperty.PropertyType));
                    }

                }
            }

            _dbContext.SaveChanges();
        }
    }
}