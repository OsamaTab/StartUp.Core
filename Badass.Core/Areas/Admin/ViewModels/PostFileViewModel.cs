using Badass.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Areas.Admin.ViewModels
{
    public class PostFileViewModel : Post
    {
        public IFormFile Photo { get; set; }
    }
}
