using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Services.Interfaces;
using Test.Core.ViewModels.Location;
using Test.Domain.Interfaces;
using Test.Domain.Models.Location;

namespace Test.Core.Services.Implementation
{
    public class LocationService : ILocationService
    {

        #region Dependancy Injection

        private readonly ILocationRepository _locationRepository;
        private readonly IAccountRepository _accountRepository;

        public LocationService(ILocationRepository locationRepository, IAccountRepository accountRepository)
        {
            _locationRepository = locationRepository;
            _accountRepository = accountRepository;
        }

        #endregion


        #region Basics

        public async Task<MainLocationViewModel> GetLocationById(int locationId)
        {
            var location = await _locationRepository.GetLocationById(locationId);
            return new MainLocationViewModel()
            {
                IsDelete = location.IsDelete,
                LocationId = location.LocationId,
                LocationTitle = location.LocationTitle,
                ParentId = location.ParentId
            };
        }

        public IEnumerable<MainLocationViewModel> GetAllLocations()
        {
            var allLocations = _locationRepository.GetAllLocations();
            IEnumerable<MainLocationViewModel> locations = allLocations.Select(l => new MainLocationViewModel()
            {
                IsDelete = l.IsDelete,
                LocationId = l.LocationId,
                LocationTitle = l.LocationTitle,
                ParentId = l.ParentId
            });

            return locations;
        }

        public async Task DeleteLocationById(int locationId)
        {
            await _locationRepository.DeleteLocationById(locationId);
            await _locationRepository.SaveAsync();
        }

        public async Task UnDeleteLocation(int locationId)
        {
            await _locationRepository.UnDeleteLocation(locationId);
            await _locationRepository.SaveAsync();
        }

        public async Task AddLocation(LocationViewModel location)
        {
            Location newLocation = new Location();
            newLocation.IsDelete = false;
            newLocation.LocationTitle = location.AddLocation.LocationTitle;

            newLocation.ParentId = location.AddLocation.CityId == 0 ? location.AddLocation.ProvinceId : location.AddLocation.CityId;

            await _locationRepository.AddLocation(newLocation);
            await _locationRepository.SaveAsync();
        }

        public async Task AddProvince(string newProvince)
        {
            Location province = new Location()
            {
                ParentId = null,
                IsDelete = false,
                LocationTitle = newProvince,
            };
            await _locationRepository.AddLocation(province);
            await _locationRepository.SaveAsync();
        }

        public async Task EditLocation(Location location)
        {
            _locationRepository.EditLocation(location);
            await _locationRepository.SaveAsync();

        }

        #endregion


        #region Get Locations

        public IEnumerable<MainLocationViewModel> GetAllProvinces()
        {
            var allProvinces = _locationRepository.GetAllLocations().Where(l => l.ParentId == null);

            IEnumerable<MainLocationViewModel> provinces = allProvinces.Select(l => new MainLocationViewModel()
            {
                IsDelete = l.IsDelete,
                LocationId = l.LocationId,
                ParentId = l.ParentId,
                LocationTitle = l.LocationTitle
            });

            return provinces;
        }
        public IEnumerable<MainLocationViewModel> GetAllCitiesByProvinceId(int provinceId)
        {
            var allCities = _locationRepository.GetAllLocations().Where(l => l.ParentId == provinceId);

            IEnumerable<MainLocationViewModel> cities = allCities.Select(l => new MainLocationViewModel()
            {
                IsDelete = l.IsDelete,
                LocationId = l.LocationId,
                ParentId = l.ParentId,
                LocationTitle = l.LocationTitle
            });

            return cities;
        }
        public IEnumerable<MainLocationViewModel> GetAllLocationsByCityId(int cityId)
        {
            var allLocations = _locationRepository.GetAllLocations().Where(l => l.ParentId == cityId);

            IEnumerable<MainLocationViewModel> cities = allLocations.Select(l => new MainLocationViewModel()
            {
                IsDelete = l.IsDelete,
                LocationId = l.LocationId,
                ParentId = l.ParentId,
                LocationTitle = l.LocationTitle
            });

            return cities;
        }
        public List<MainLocationViewModel> GetAllCitiesForSelectListByProvinceId(int provinceId)
        {
            var allCities = _locationRepository.GetAllLocations().Where(l => l.ParentId == provinceId);

            MainLocationViewModel newMainLocation = new MainLocationViewModel()
            {
                ParentId = 0,
                LocationTitle = "***** شهر جدید *****"
            };

            List<MainLocationViewModel> cities = allCities.Select(l => new MainLocationViewModel()
            {
                IsDelete = l.IsDelete,
                LocationId = l.LocationId,
                ParentId = l.ParentId,
                LocationTitle = l.LocationTitle
            }).ToList();

            cities.Add(newMainLocation);

            return cities;
        }
        public List<MainLocationViewModel> GetNullLocationForSelectList()
        {
            List<MainLocationViewModel> newMainLocations = new List<MainLocationViewModel>();

            MainLocationViewModel choiceRequest = new MainLocationViewModel()
            {
                ParentId = 0,
                LocationTitle = "-- لطفا انتخاب کنید --"
            };

            MainLocationViewModel selfCity = new MainLocationViewModel()
            {
                ParentId = 0,
                LocationTitle = "شهر جدید"
            };

            newMainLocations.Add(choiceRequest);
            newMainLocations.Add(selfCity);

            return newMainLocations;
        }
        public LocationViewModel GetLocationForView()
        {
            var allLocations = _locationRepository.GetAllLocations().ToList();

            List<LocationListViewModel> locations = allLocations.Select(l => new LocationListViewModel()
            {
                LocationId = l.LocationId,
                ParentId = l.ParentId,
                LocationTitle = l.LocationTitle,
                IsDelete = l.IsDelete
            }).ToList();

            return new LocationViewModel()
            {
                LocationList = locations
            };
        }
        public async Task<MainLocationViewModel> GetProvinceByCityId(int cityId)
        {
            var city = await _locationRepository.GetLocationById(cityId);
            var province = _locationRepository.GetAllLocations().SingleOrDefault(l => l.LocationId == city.ParentId);
            return new MainLocationViewModel()
            {
                IsDelete = province.IsDelete,
                LocationId = province.LocationId,
                ParentId = province.ParentId,
                LocationTitle = province.LocationTitle
            };
        }

        #endregion


        public async Task ChangeUserCity(Guid userId,int locationId)
        {
            await _accountRepository.EditUserCity(userId, locationId);
            await _accountRepository.SaveAsync();
        }

    }
}
