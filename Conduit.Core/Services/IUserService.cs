using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Core.Models;

namespace Conduit.Core.Services
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(int id);
        Task<User> GetByUsername(string username);
        Task<User> GetByUsernameWithFollowers(string username);
        Task<User> CreateUser(User user);
        Task<bool> IsUniqueEmail(string email);
        Task<bool> IsUniqueUsername(string username, string exception = "");
        Task UpdateUser(User userOld, User userNew);
        Task UpdatePassword(User user, string newPassword);
        Task AddFollower(int currentUserId, User followedUser);
        Task DeleteFollower(int currentUserId, User followedUser);
        bool IsFollowing(int currentUserId, User followingUser);
    }
}