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
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFollower(int currentUserId, User followedUser)
        {
            var user = new User { UserId = currentUserId };
            _context.Users.Attach(user);
            followedUser.Followers.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteFollower(int currentUserId, User followedUser)
        {
            followedUser.Followers.Remove(followedUser.Followers.Single(x => x.UserId == currentUserId));
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetByUsernameWithFollowers(string username)
        {
            return await _context.Users.Include(x => x.Followers).FirstOrDefaultAsync(x => x.Username == username);
        }

        public bool IsFollowing(int currentUserId, User followingUser)
        {
            return followingUser.Followers.Any(x => x.UserId == currentUserId);
        }

        public async Task<bool> IsUniqueEmail(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsUniqueUsername(string username, string exception = "")
        {
            return await _context.Users.AnyAsync(x => x.Username == username && x.Username != exception);
        }

        public async Task UpdatePassword(User user, string newPassword)
        {
            user.Password = newPassword;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User userOld, User userNew)
        {
            userOld.Username = userNew.Username;
            userOld.Bio = userNew.Bio;

            await _context.SaveChangesAsync();
        }
    }
}