using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Models.BusinessDays;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.SpaServices;
using SugarMaMa.API.Models.Shifts;

namespace SugarMaMa.API.Services
{
    public class EstheticianService : IEstheticianService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<Esthetician> _estheticians;
        private readonly IRepository<Shift> _shifts;
        private readonly IRepository<BusinessDay> _businessDays;


        public EstheticianService(IRepository<Esthetician> estheticians, IRepository<Shift> shifts, IRepository<BusinessDay> businessDays,
            IMapper mapper, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _estheticians = estheticians;
            _shifts = shifts;
            _businessDays = businessDays;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<EstheticianViewModel>> GetEstheticiansAsync()
        {

            var list = new List<EstheticianViewModel>();
            var estheticians = await _estheticians.GetAsync(x => x.User, x => x.Services);

            foreach (var esthetician in estheticians)
            {
                try
                {
                    var shifts = await _shifts.FindAsync(x => x.EstheticianId == esthetician.Id, x => x.BusinessDay, x => x.BusinessDay.Location);

                    var toAdd = new EstheticianViewModel
                    {
                        FirstName = esthetician.User.FirstName,
                        Services = _mapper.Map<List<SpaServiceViewModel>>(esthetician.Services),
                        Shifts = new List<ShiftViewModel>(),
                        Id = esthetician.Id
                    };

                    foreach (var shift in shifts)
                    {
                        toAdd.Shifts.Add(new ShiftViewModel
                        {
                            StartTime = shift.StartTime,
                            EndTime = shift.EndTime,
                            BusinessDay = new BusinessDayViewModel
                            {
                                ClosingTime = shift.BusinessDay.ClosingTime,
                                OpeningTime = shift.BusinessDay.OpeningTime,
                                DayOfWeek = shift.BusinessDay.DayOfWeek.ToString("G"),
                                Id = shift.BusinessDay.Id,
                                Location = new LocationViewModel
                                {
                                    Id = shift.BusinessDay.Location.Id,
                                    City = shift.BusinessDay.Location.City
                                }
                            }
                        });
                    }
                    list.Add(toAdd);
                } 
                catch(TaskCanceledException e)
                {
                    var a = e;
                }
            }
            return list;
        }

        public async Task<List<EstheticianAdminViewModel>> GetEstheticianMasterListAsync()
        {
            var estheticians = await _estheticians.GetAsync(x => x.User, x => x.Services);
            var list = new List<EstheticianAdminViewModel>();

            foreach (var esthetician in estheticians)
            {
                var toAdd = new EstheticianAdminViewModel
                {
                    Id = esthetician.Id,
                    FirstName = esthetician.User.FirstName,
                    LastName = esthetician.User.LastName,
                    Services = new List<SpaServiceViewModel>(),
                    Color = esthetician.Color
                };

                foreach (var service in esthetician.Services)
                    toAdd.Services.Add(new SpaServiceViewModel { Id = service.Id, Name = service.Name });

                list.Add(toAdd);
            }

            return list;
        }

        public async Task<EstheticianAdminViewModel> GetByIdAsync(int id)
        {
            var esthetician = await _estheticians.GetByIdAsync(id, x => x.User, x => x.Services, x => x.Shifts);
            var businessDays =
                _businessDays.FindAsync(x => esthetician.Shifts.Select(s => s.BusinessDayId).Contains(x.Id));

            var retVal = new EstheticianAdminViewModel
            {
                Id = esthetician.Id,
                FirstName = esthetician.User.FirstName,
                LastName = esthetician.User.LastName,
                Services = new List<SpaServiceViewModel>(),
                Color = esthetician.Color,
                Shifts = new List<ShiftViewModel>()
            };

            foreach (var service in esthetician.Services)
                retVal.Services.Add(new SpaServiceViewModel { Id = service.Id, Name = service.Name });

            var bdList = await businessDays;

            foreach (var shift in esthetician.Shifts)
            {
                var bd = bdList.Single(x => x.Id == shift.BusinessDayId);

                retVal.Shifts.Add(new ShiftViewModel
                {
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime,
                    BusinessDay = new BusinessDayViewModel
                    {
                        OpeningTime = bd.OpeningTime,
                        ClosingTime = bd.ClosingTime,
                        Location = new LocationViewModel
                        {
                            Id = bd.LocationId,
                            City = bd.LocationId == 1 ? "Stanton" : "Brea"
                        }
                    }
                });
            }
            return retVal;
        }

        public async Task<Esthetician> AddEstheticianAsync(AddEstheticianViewModel model)
        {
            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, "$ugar4me");

            if (!result.Succeeded)
                //TODO: Logging!
                return null;

            var esth = new Esthetician
            {
                ApplicationUserId = user.Id
            };

            var roleResult = _userManager.AddToRoleAsync(user, "Esthetician").Result;
            return _estheticians.AddAsync(esth).Result;
        }

        public async Task<ShiftViewModel> GetShiftById(int id)
        {
            var shift = await _shifts.GetByIdAsync(id, x => x.BusinessDay, x => x.BusinessDay.Location);

            return new ShiftViewModel
            {
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                BusinessDay = new BusinessDayViewModel
                {
                    Id = shift.BusinessDayId,
                    DayOfWeek = shift.BusinessDay.DayOfWeek.ToString("G"),
                    OpeningTime = shift.BusinessDay.OpeningTime,
                    ClosingTime = shift.BusinessDay.ClosingTime,
                    Location = new LocationViewModel
                    {
                        Id = shift.BusinessDay.LocationId,
                        Address1 = shift.BusinessDay.Location.Address1,
                        Address2 = shift.BusinessDay.Location.Address2,
                        City = shift.BusinessDay.Location.City,
                        State = shift.BusinessDay.Location.State,
                        ZipCode = shift.BusinessDay.Location.ZipCode,
                        PhoneNumber = shift.BusinessDay.Location.PhoneNumber
                    }
                }
            };
        }

        public async Task<List<ShiftViewModel>> GetShiftsByEstheticianId(int id)
        {
            var list = await _shifts.FindAsync(x => x.EstheticianId == id, x => x.BusinessDay, x => x.BusinessDay.Location);
            var shifts = new List<ShiftViewModel>();

            foreach (var shift in list)
            {
                shifts.Add(new ShiftViewModel
                {
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime,
                    BusinessDay = new BusinessDayViewModel
                    {
                        Id = shift.BusinessDayId,
                        DayOfWeek =  shift.BusinessDay.DayOfWeek.ToString("G"),
                        OpeningTime = shift.BusinessDay.OpeningTime,
                        ClosingTime = shift.BusinessDay.ClosingTime,
                        Location = new LocationViewModel
                        {
                            Id = shift.BusinessDay.LocationId,
                            Address1 = shift.BusinessDay.Location.Address1,
                            Address2 = shift.BusinessDay.Location.Address2,
                            City = shift.BusinessDay.Location.City,
                            State = shift.BusinessDay.Location.State,
                            ZipCode = shift.BusinessDay.Location.ZipCode,
                            PhoneNumber = shift.BusinessDay.Location.PhoneNumber
                        }
                    }
                });    
            }
            return shifts;
        }
    }

    public interface IEstheticianService
    {
        Task<List<EstheticianAdminViewModel>> GetEstheticianMasterListAsync();
        Task<List<EstheticianViewModel>> GetEstheticiansAsync();
        Task<EstheticianAdminViewModel> GetByIdAsync(int id);
        Task<Esthetician> AddEstheticianAsync(AddEstheticianViewModel model);
        Task<ShiftViewModel> GetShiftById(int id);
        Task<List<ShiftViewModel>> GetShiftsByEstheticianId(int id);
    }
}
