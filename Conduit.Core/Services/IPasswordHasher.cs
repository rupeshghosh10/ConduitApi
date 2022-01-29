using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        byte[] GenerateSalt();
        bool VerifyPassword(string password);
    }
}