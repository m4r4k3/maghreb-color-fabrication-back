using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.model;


namespace fabrication_maghreb_color.service
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
                _logger.LogError(err.ToString());
                return false;
            }
        }
    }
}