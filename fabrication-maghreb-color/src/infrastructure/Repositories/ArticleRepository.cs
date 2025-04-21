using fabrication_maghreb_color.application.Interfaces;
using fabrication_maghreb_color.Config.Contexts;
using fabrication_maghreb_color.Infrastructure.model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fabrication_maghreb_color.application.repository
{


    public class ArticleRepository : IArticleRepository
    {
        private readonly SageContext _sageDbContext;

        public ArticleRepository(SageContext context)
        {
            _sageDbContext = context;
        }

        public List<SageArticle> GetFilteredArticles(string filter)
        {
            return _sageDbContext.SageArticleDbo
                .Where(e => e.codeFamille == filter)
                .ToList();
        }

        public async Task<List<SageArticle>> GetFilteredArticlesAsync(string filter)
        {
            return await _sageDbContext.SageArticleDbo
                .Where(e => e.codeFamille == filter)
                .ToListAsync();
        }
    }
}