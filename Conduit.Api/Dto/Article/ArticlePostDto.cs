using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Conduit.Api.Dto.Tag;

namespace Conduit.Api.Dto.Article
{
    public class ArticlePostDto
    {   
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Body { get; set; }

        public IEnumerable<TagDto> Tags { get; set; }
    }
}