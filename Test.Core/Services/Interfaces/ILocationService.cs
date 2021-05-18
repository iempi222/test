using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Core.ViewModels.Location;
using Test.Domain.Models.Location;

namespace Test.Core.Services.Interfaces
{
    public interface ILocationService
    {

        #region Basics

        Task<MainLocationViewModel> GetLocationById(int locationId);
        IEnumerable<MainLocationViewModel> GetAllLocations();
        Task DeleteLocationById(int locationId);
        Task UnDeleteLocation(int locationId);
        Task AddLocation(LocationViewModel location);
        Task AddProvince(string newProvince);
        Task EditLocation(Location location);

        #endregion

        #region Get Locations

        IEnumerable<MainLocationViewModel> GetAllProvinces();
        IEnumerable<MainLocationViewModel> GetAllCitiesByProvinceId(int provinceId);
        List<MainLocationViewModel> GetAllCitiesForSelectListByProvinceId(int provinceId);
        List<MainLocationViewModel> GetNullLocationForSelectList();
        IEnumerable<MainLocationViewModel> GetAllLocationsByCityId(int cityId);
        LocationViewModel GetLocationForView();
        Task ChangeUserCity(Guid userId, int locationId);
        Task<MainLocationViewModel> GetProvinceByCityId(int cityId);

        #endregion

    }
}
