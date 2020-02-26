using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Badass.Core.Controllers.Admin
{
    [Route("admin/{controller}/{action=Index}/{id?}")]
    public class BaseController : Controller
    {
       
    }
}