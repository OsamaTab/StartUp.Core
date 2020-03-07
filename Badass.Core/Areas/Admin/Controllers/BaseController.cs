using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badass.Core.Areas.Admin
{
    [Area("admin")]
    [Authorize(Roles = "Admin , SuperAdmin")]
    public class BaseController : Controller
    {
    }
}