using fabrication_maghreb_color.Infrastructure.model;

namespace fabrication_maghreb_color.application.Interfaces
{
    public interface IArticleService
    {
        List<SageArticle> GetFilteredArticles(string filter);
        Task<List<SageArticle>> GetFilteredArticlesAsync(string filter);
    }

}