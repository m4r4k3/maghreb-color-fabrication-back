using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{
    public class ProjetService
    {
        private readonly MainContext _dbContext;
        private readonly ILogger<ProjetService> _logger;
        public ProjetService(MainContext context, ILogger<ProjetService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        public bool create(Projet projet)
        {
            try
            {
                _dbContext.ProjetDbo.Add(projet);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                _logger.LogError(err.ToString());
                return false;
            }
        }
        public List<TypeProjet> GetAllTypes()
        {
            return _dbContext.typeProjetDbo.ToList();
        }
        public List<Projet> GetAll(int? type)
        {
            if (type.HasValue)
            {
                return _dbContext.ProjetDbo.Include(p => p.preparationFabrication) .Where(e => e.TypeProjet == type.Value).ToList();
            }
            else
            {
                return _dbContext.ProjetDbo.Include(p => p.preparationFabrication) .ToList();
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
            Console.WriteLine(propertyName + " " + propertyValue + " " + projectProperty.PropertyType);
            
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
            
            Console.WriteLine("reached here ");
        }
    }
    
    _dbContext.SaveChanges();
}
    }
}