using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;

namespace Conduit.Core.Services
{
    public interface IArticleService
    {
        Task<Article> CreateArticle(Article article, int authorId);
        Task<ICollection<Article>> GetArticles(string tag, string author, int limit, int offset);
        Task<Article> GetArticle(string slug);
        Task<Article> UpdateArticle(Article oldArticle, Article newArticle);
    }
}