using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;
using Conduit.Core.Services;
using Conduit.Data;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Service
{
    public class ArticleManager : IArticleManager
    {
        private readonly ApplicationDbContext _context;

        public ArticleManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Article> CreateArticle(Article article, int authorId)
        {
            article.CreatedAt = DateTime.UtcNow;
            article.AuthorId = authorId;

            var tags = await _context.Tags.ToListAsync();
            var articleTags = article.Tags.ToList();
            foreach (var tag in articleTags)
            {
                if (tags.Any(x => x.Text == tag.Text))
                {
                    article.Tags.Remove(tag);
                    int tagId = tags.FirstOrDefault(x => x.Text == tag.Text).TagId;
                    var tagPlaceHolder = _context.Tags.Local.FirstOrDefault(x => x.TagId == tagId) ?? new Tag { TagId = tagId };
                    _context.Tags.Attach(tagPlaceHolder);
                    article.Tags.Add(tagPlaceHolder);
                }
            }

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            try
            {
                article.Slug = $"{article.Title.Replace(" ", "-").ToLower()}";
                await _context.SaveChangesAsync();
            }
            catch
            {
                article.Slug = $"{article.Title.Replace(" ", "-").ToLower()}-{article.ArticleId}";
                await _context.SaveChangesAsync();
            }

            return article;
        }

        public async Task<ICollection<Article>> GetArticles(string tag, string author, int limit, int offset)
        {
            return await _context.Articles
                .OrderBy(x => x.ArticleId)
                .Skip(offset)
                .Take(limit)
                .Where(x => x.Tags.Any(x => tag == "" ? true : x.Text == tag))
                .Where(x => author == "" ? true : x.Author.Username == author)
                .ToListAsync();
        }
    }
}