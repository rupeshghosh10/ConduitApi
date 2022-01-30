using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Api.Dto.User
{
    public class UserPutDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(200)]
        public string Bio { get; set; }
    }
}