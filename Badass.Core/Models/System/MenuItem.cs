using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Models.System
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
        public int MenuId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
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
