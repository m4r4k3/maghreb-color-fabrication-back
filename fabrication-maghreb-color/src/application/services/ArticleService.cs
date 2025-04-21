using fabrication_maghreb_color.Infrastructure.model;
using fabrication_maghreb_color.application.repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fabrication_maghreb_color.application.Interfaces;

namespace fabrication_maghreb_color.Application.Services
{

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(
            IArticleRepository articleRepository,
            ILogger<ArticleService> logger)
        {
            _articleRepository = articleRepository;
            _logger = logger;
        }

        public List<SageArticle> GetFilteredArticles(string filter)
        {
            try
            {
                _logger.LogInformation($"Fetching articles with filter: {filter}");
                return _articleRepository.GetFilteredArticles(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching articles with filter: {filter}");
                throw;
            }
        }

        public async Task<List<SageArticle>> GetFilteredArticlesAsync(string filter)
        {
            try
            {
                _logger.LogInformation($"Fetching articles asynchronously with filter: {filter}");
                return await _articleRepository.GetFilteredArticlesAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching articles asynchronously with filter: {filter}");
                throw;
            }
        }
    }
}