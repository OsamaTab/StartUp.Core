using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int PostTypeId { get; set; }

        [ForeignKey("PostTypeId")]
        public PostType PostType { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public IdentityUser CreatedBy { get; set; }
        public int? CreatedByUserId { get; set; }
        public IdentityUser UpdatedBy { get; set; }
        public int? UpdatedByUserId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
