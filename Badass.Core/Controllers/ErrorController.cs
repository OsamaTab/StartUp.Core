using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Badass.Core.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{Stats}")]
        public IActionResult Index(int Stats)
        {
            switch (Stats)
            {
                case 404:
                    ViewBag.MessageError = "Sorre Page Not Fount 404";
                    break;
                default:
                    break;
            }
            return View("Error");
        }
    }
}