using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Core.Services
{
    public interface IPasswordManager
    {
        bool VerifyPassword(string password, string passwordInDb);
        string GeneratePassword(string password);
    }
}