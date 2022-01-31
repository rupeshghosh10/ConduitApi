using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Api.Dto.User
{
    public class UserResetPasswordDto
    {
        [Required]
        [MaxLength(50)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(50)]
        public string NewPassword { get; set; }
    }
}