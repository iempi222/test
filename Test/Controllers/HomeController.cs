using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Core.Services.Interfaces;
using Test.Extensions;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {

        #region Dependancy Injection

        private readonly ILogger<HomeController> _logger;
        private readonly ILocationService _locationService;

        public HomeController(ILogger<HomeController> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectCity()
        {
            var allLocations = _locationService.GetAllLocations();
            return View(allLocations);
        }

        [HttpPost]
        public async Task<IActionResult> SelectCity(int id)
        {
            
            //await _locationService.ChangeUserCity(userId, id);
            return RedirectToAction("Index");
        }

        public IActionResult BestMarkets()
        {
            return View();
        }

        #region System Default

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
