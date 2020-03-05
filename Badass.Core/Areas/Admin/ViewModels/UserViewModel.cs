using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badass.Core.Areas.Admin.ViewModels
{
    public class UserViewModel { 
   
        public string UserId { get; set; }
        public string RoleId { get; set; }
        
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
