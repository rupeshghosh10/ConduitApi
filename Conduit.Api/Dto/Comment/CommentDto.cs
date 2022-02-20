using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Api.Dto.Profile;

namespace Conduit.Api.Dto.Comment
{
    public class CommentDto
    {
        public int CommentId { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ProfileDto Author { get; set; }
    }
}