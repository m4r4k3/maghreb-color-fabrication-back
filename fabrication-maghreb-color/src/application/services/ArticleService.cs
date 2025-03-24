using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;


namespace fabrication_maghreb_color.application.service
{
    public class ArticleService
    {
        private readonly SageContext _SageDbContext;
        private readonly MainContext _DevContext;
        private readonly ILogger<ArticleService> _logger;
        public ArticleService(SageContext context, MainContext devContext, ILogger<ArticleService> logger)
        {
            _SageDbContext = context;
            _DevContext = devContext;
            _logger = logger;
        }
        public List<SageArticle> GetFilteredArticles(string filter)
        {
            return _SageDbContext.SageArticleDbo.Where(e => e.codeFamille == filter).ToList();
        }
    }
}