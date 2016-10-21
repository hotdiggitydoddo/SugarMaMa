using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Models.BusinessDays;

namespace SugarMaMa.API.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locations;

        public LocationService(IRepository<Location> locations)
        {
            _locations = locations;
        }

        public async Task<List<LocationViewModel>> GetLocationsAsync()
        {
            var list = new List<LocationViewModel>();
            var locations = await _locations.GetAsync(x => x.BusinessDays);

            foreach (var location in locations)
            {
                var businessDays = location.BusinessDays.Select(x => new BusinessDayViewModel
                {
                    Id = x.Id,
                    ClosingTime = x.ClosingTime,
                    OpeningTime = x.OpeningTime,
                    DayOfWeek = x.DayOfWeek.ToString("G"),
                });

                list.Add(new LocationViewModel
                {
                    Id = location.Id,
                    Address1 = location.Address1,
                    Address2 = location.Address2,
                    City = location.City,
                    State = location.State,
                    ZipCode = location.ZipCode,
                    PhoneNumber = location.PhoneNumber,
                    BusinessDays = businessDays.ToList()
                } );
            }
            return list;
        }
    }

    public interface ILocationService
    {
        Task<List<LocationViewModel>> GetLocationsAsync();
    }
}
