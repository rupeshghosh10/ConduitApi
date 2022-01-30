using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Services
{
    public interface IPasswordManager
    {
        string HashPassword(string password, string salt);
        string GenerateSalt();
        bool VerifyPassword(string password, string passwordInDb, string salt);
    }
}