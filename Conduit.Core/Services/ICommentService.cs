using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;

namespace Conduit.Core.Services
{
    public interface ICommentService
    {
        Task<Comment> AddComment(Comment comment, int articleID, int authorId);
        Task<IEnumerable<Comment>> GetComments(string slug);
        Task<Comment> GetComment(int id);
        Task DeleteComment(Comment comment);
    }
}