using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Areas.Admin.ViewModels
{
    public class PostFileViewModel
    {
        public int PostTypeId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
        public IFormFile Photo { get; set; }
    }
}
