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

namespace SugarMaMa.API.Services
{
    public class EstheticianService : IEstheticianService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<Esthetician, int> _estheticians;


        public EstheticianService(IRepository<Esthetician, int> estheticians, IMapper mapper,
            RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _estheticians = estheticians;
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
                var toAdd = new EstheticianViewModel
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
        Task<List<EstheticianViewModel>> GetEstheticiansAsync();
        Task<Esthetician> AddEstheticianAsync(AddEstheticianViewModel model);
    }
}
