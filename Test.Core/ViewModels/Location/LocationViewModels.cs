using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Test.Core.ViewModels.Location
{

    public class LocationListViewModel
    {
        [AllowNull]
        public int LocationId { get; set; }
        [Display(Name = "عنوان مکان")]
        [AllowNull]
        public string LocationTitle { get; set; }
        [Display(Name = "مکان اصلی")]
        [AllowNull]
        public int? ParentId { get; set; }

        public bool IsDelete { get; set; }
    }

    public class AddLocationViewModel
    {
        [Display(Name = "عنوان مکان")]
        public string LocationTitle { get; set; }
        [Display(Name = "استان")]
        public int ProvinceId { get; set; }
        [Display(Name = "شهر")]
        public int? CityId { get; set; }
    }

    public class LocationViewModel
    {
        public List<LocationListViewModel> LocationList { get; set; }
        public AddLocationViewModel AddLocation { get; set; }
    }

    public class MainLocationViewModel
    {
        public int LocationId { get; set; }
        public string LocationTitle { get; set; }
        public int? ParentId { get; set; }
        public bool IsDelete { get; set; }
    }
}
