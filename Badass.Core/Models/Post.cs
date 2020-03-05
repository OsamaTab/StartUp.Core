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
        [ForeignKey("CreatedByUserId")]
        public ApplicationUser CreatedBy { get; set; }
        public string? CreatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public ApplicationUser UpdatedBy { get; set; }
        public string? UpdatedByUserId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
