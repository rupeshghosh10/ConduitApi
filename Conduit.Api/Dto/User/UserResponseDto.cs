using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Api.Dto.User
{
    public class UserResponseDto
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Username { get; set; }
        
        public string Bio { get; set; }
    }
}