using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Test.Domain.Interfaces;
using Test.Domain.Models.Location;
using Test.Infra.Data.Context;

namespace Test.Infra.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        #region Dependancy Injection

        private readonly TestDbContext _context;

        public LocationRepository(TestDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddLocation(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        public async Task DeleteLocationById(int locationId)
        {
            var location = await GetLocationById(locationId);
            location.IsDelete = true;
        }

        public void EditLocation(Location location)
        {
            _context.Locations.Update(location);
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _context.Locations;
        }

        public async Task<Location> GetLocationById(int locationId)
        {
            return await _context.Locations.SingleOrDefaultAsync(l => l.LocationId == locationId);
        }

        public async Task UnDeleteLocation(int locationId)
        {
            var location = await GetLocationById(locationId);
            location.IsDelete = false;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
