using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Models.Location;

namespace Test.Domain.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location> GetLocationById(int locationId);
        IEnumerable<Location> GetAllLocations();
        Task DeleteLocationById(int locationId);
        Task UnDeleteLocation(int locationId);
        Task AddLocation(Location location);
        void EditLocation(Location location);

        Task SaveAsync();
    }
}
