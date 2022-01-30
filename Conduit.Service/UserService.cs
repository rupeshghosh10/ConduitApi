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

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsUniqueEmail(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsUniqueUsername(string username, string exception = "")
        {
            return await _context.Users.AnyAsync(x => x.Username == username && x.Username != exception);
        }   

        public async Task UpdateUser(User userOld, User userNew)
        {
            userOld.Username = userNew.Username;
            userOld.Bio = userNew.Bio;

            await _context.SaveChangesAsync();
        }
    }
}