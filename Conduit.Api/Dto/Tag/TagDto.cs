using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Api.Dto.Tag
{
    public class TagDto
    {   
        [Required]
        [MaxLength(50)]
        public string Text { get; set; }
    }
}