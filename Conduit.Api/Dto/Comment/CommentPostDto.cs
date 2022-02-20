using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Api.Dto.Comment
{
    public class CommentPostDto
    {
        [Required]
        [MaxLength(250)]
        public string Body { get; set; }
    }
}