using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Test.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult WelcomePage()
        {
            return View();
        }


        public IActionResult Index(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("WelcomePage");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
