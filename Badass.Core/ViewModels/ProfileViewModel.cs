using Badass.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.ViewModels
{
    public class ProfileViewModel : ApplicationUser
    {
        public IFormFile Photo { get; set; }
    }
}
