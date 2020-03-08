using Badass.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Post>  Posts{ get; set; }
        public ContactUs ContactUs { get; set; }
    }
}
