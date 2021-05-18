using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Test.Core.Services.Interfaces;
using Test.Core.ViewModels.Location;
using Test.Extensions;

namespace Test.Areas.Admin.Controllers
{
    public class LocationController : BaseController
    {

        #region Dependancy Injection

        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        #endregion

        public IActionResult LocationList()
        {
            ViewData["ProvincesSelectList"] = new SelectList(_locationService.GetAllProvinces(), "LocationId", "LocationTitle");
            var allLocations = _locationService.GetLocationForView();
            return View(allLocations);
        }

        [HttpPost]
        public async Task<IActionResult> LocationList(LocationViewModel location)
        {

            if (!ModelState.IsValid)
            {
                HttpContext.SetMessage(ActionMessageType.Error, "ورودی ها نامعتبر میباشند");
                return RedirectToAction("LocationList");
            }

            if (location.AddLocation.ProvinceId == 0)
            {
                HttpContext.SetMessage(ActionMessageType.Error, "ابتدا استان را انتخاب کنید");
                return RedirectToAction("LocationList");
            }

            await _locationService.AddLocation(location);
            HttpContext.SetMessage(ActionMessageType.Success, "ثبت شد");

            return RedirectToAction("LocationList");
        }

        public IActionResult AddProvince()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProvince(string province)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.SetMessage(ActionMessageType.Error, "ورودی نامعتبر میباشند");
                return RedirectToAction("AddProvince");
            }

            if (province == null)
            {
                HttpContext.SetMessage(ActionMessageType.Error, "ورودی نمیتواند خالی باشد");
                return RedirectToAction("AddProvince");
            }

            await _locationService.AddProvince(province);
            HttpContext.SetMessage(ActionMessageType.Success, "استان جدید افزوده شد");
            return RedirectToAction("LocationList");
        }

        [HttpGet]
        public JsonResult GetCities(int id)
        {
            if (id == 0)
            {
                var result = _locationService.GetNullLocationForSelectList().ToList();
                return Json(new SelectList(result, "LocationId", "LocationTitle"));
            }
            else
            {
                var result = _locationService.GetAllCitiesForSelectListByProvinceId(id);
                return Json(new SelectList(result, "LocationId", "LocationTitle"));
            }
        }

        public async Task DeleteLocation(int id)
        {
            await _locationService.DeleteLocationById(id);
        }

        public async Task UnDeleteLocation(int id)
        {
            await _locationService.UnDeleteLocation(id);
        }

    }
}
