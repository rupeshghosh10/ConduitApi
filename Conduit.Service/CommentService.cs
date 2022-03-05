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
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddComment(Comment comment, int articleID, int authorId)
        {
            comment.ArticleId = articleID;
            comment.AuthorId = authorId;
            comment.CreatedAt = DateTime.UtcNow;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            comment.Author = await _context.Users.FindAsync(authorId);

            return comment;
        }

        public async Task DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetComment(int id)
        {
            return await _context.Comments
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.CommentId == id);
        }

        public async Task<IEnumerable<Comment>> GetComments(string slug)
        {
            return await _context.Comments
                .Include(x => x.Author)
                .ThenInclude(x => x.Followers)
                .Where(x => x.Article.Slug == slug)
                .OrderBy(x => x.CommentId)
                .ToListAsync();
        }
    }
}