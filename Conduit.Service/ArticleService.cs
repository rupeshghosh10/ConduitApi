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
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
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
                    var tagPlaceHolder = _context.Tags.Local.FirstOrDefault(x => x.TagId == tagId);
                    _context.Tags.Attach(tagPlaceHolder);
                    article.Tags.Add(tagPlaceHolder);
                }
            }

            await _context.Articles.AddAsync(article);

            try
            {
                article.Slug = $"{article.Title.Replace(" ", "-").ToLower()}";
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _context.SaveChangesAsync();
                article.Slug = $"{article.Title.Replace(" ", "-").ToLower()}-{article.ArticleId}";
                await _context.SaveChangesAsync();
            }

            article.Author = await _context.Users.FindAsync(authorId);

            return article;
        }

        public async Task DeleteArticle(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }

        public async Task FavoriteArticle(Article article, int currentUserId)
        {
            var userPlaceholder = _context.Users.Local.FirstOrDefault(x => x.UserId == currentUserId) ?? new User { UserId = currentUserId };
            _context.Users.Attach(userPlaceholder);
            article.FavoritedUsers.Add(userPlaceholder);
            await _context.SaveChangesAsync();
        }

        public async Task<Article> GetArticle(string slug)
        {
            return await _context.Articles
                .Include(x => x.Author)
                .ThenInclude(x => x.Followers)
                .Include(x => x.Tags)
                .Include(x => x.FavoritedUsers)
                .FirstOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<ICollection<Article>> GetArticles(string tag, string author, string favorited, int limit, int offset)
        {
            return await _context.Articles
                .Include(x => x.Author)
                .ThenInclude(x => x.Followers)
                .Include(x => x.Tags)
                .Include(x => x.FavoritedUsers)
                .OrderBy(x => x.ArticleId)
                .Where(x => x.Tags.Any(x => x.Text.ToLower().StartsWith(tag.ToLower())))
                .Where(x => author == "" || x.Author.Username.ToLower() == author.ToLower())
                .Where(x => favorited == "" || x.FavoritedUsers.Any(x => x.Username == favorited))
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<ICollection<Article>> GetArticlesFeed(int limit, int offset, int currentUserId)
        {
            return await _context.Articles
                .Include(x => x.Author)
                .ThenInclude(x => x.Followers)
                .Include(x => x.Tags)
                .Include(x => x.FavoritedUsers)
                .OrderBy(x => x.ArticleId)
                .Where(x => x.Author.Followers.Any(y => y.UserId == currentUserId))
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public bool IsFavourite(Article article, int currentUserId)
        {
            return article.FavoritedUsers.Any(x => x.UserId == currentUserId);
        }

        public async Task UnFavoriteArticle(Article article, int currentUserId)
        {
            var userPlaceholder = _context.Users.Local.FirstOrDefault(x => x.UserId == currentUserId) ?? new User { UserId = currentUserId };
            _context.Users.Attach(userPlaceholder);
            article.FavoritedUsers.Remove(userPlaceholder);
            await _context.SaveChangesAsync();
        }

        public async Task<Article> UpdateArticle(Article oldArticle, Article newArticle)
        {
            oldArticle.Title = newArticle.Title;
            oldArticle.Description = newArticle.Description;
            oldArticle.Body = newArticle.Body;
            oldArticle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return oldArticle;
        }
    }
}