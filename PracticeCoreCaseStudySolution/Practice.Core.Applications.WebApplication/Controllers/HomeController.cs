using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Practice.Core.Applications.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Message = "Professional Case Study for ASP.NET Core (From Viewbag)";

            return View();
        }

        public IActionResult ThrowError()
        {
            throw new NotImplementedException();
        }
    }
}