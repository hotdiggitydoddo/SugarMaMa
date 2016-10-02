using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.SpaServices;
using SugarMaMa.API.Models.Shifts;
using SugarMaMa.API.Models.BusinessDays;

namespace SugarMaMa.API.Services
{
    public class EstheticianService : IEstheticianService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<Esthetician, int> _estheticians;
        private readonly IRepository<Shift, Guid> _shifts;
        private readonly IRepository<BusinessDay, int> _businessDays;


        public EstheticianService(IRepository<Esthetician, int> estheticians, IRepository<Shift, Guid> shifts, IRepository<BusinessDay, int> businessDays,
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
            var estheticians = await _estheticians.GetAsync(x => x.User, x => x.Services);
            var list = new List<EstheticianViewModel>();

            foreach (var esthetician in estheticians)
            {
                var shifts = await _shifts.FindAsync(x => x.EstheticianId == esthetician.Id, x => x.BusinessDay);

                var toAdd = new EstheticianViewModel
                {
                    FirstName = esthetician.User.FirstName,
                    Services = _mapper.Map<List<SpaServiceViewModel>>(esthetician.Services),
                    Shifts = new List<ShiftViewModel>()
                };

                foreach (var shift in shifts)
                {
                    toAdd.Shifts.Add(new ShiftViewModel
                    {
                        Esthetician = toAdd,
                        StartTime = shift.StartTime,
                        EndTime = shift.EndTime,
                        BusinessDay = _mapper.Map<BusinessDayViewModel>(await _businessDays.GetByIdAsync(shift.BusinessDayId, x => x.Location))
                    });
                }

                //foreach (var service in esthetician.Services)
                //    toAdd.Services.Add(new SpaServiceViewModel { Id = service.Id, Name = service.Name });

                //foreach (var shift in esthetician.Shifts)
                //{
                //    toAdd.Shifts.Add(new ShiftViewModel {  BusinessDay = new BusinessDayViewModel {  } })
                //}

                list.Add(toAdd);
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
                    FirstName = esthetician.User.FirstName,
                    LastName = esthetician.User.LastName,
                    Services = new List<SpaServiceViewModel>()
                };

                foreach (var service in esthetician.Services)
                    toAdd.Services.Add(new SpaServiceViewModel { Id = service.Id, Name = service.Name });

                list.Add(toAdd);
            }

            return list;
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
    }

    public interface IEstheticianService
    {
        Task<List<EstheticianAdminViewModel>> GetEstheticianMasterListAsync();
        Task<List<EstheticianViewModel>> GetEstheticiansAsync();
        Task<Esthetician> AddEstheticianAsync(AddEstheticianViewModel model);
    }
}
