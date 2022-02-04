using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Api.Dto.Profile
{
    public class ProfileDto
    {
        public string Username { get; set; }

        public string Bio { get; set; }

        public bool IsFollowing { get; set; }
    }
}