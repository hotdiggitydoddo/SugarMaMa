using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.SpaServices;

namespace SugarMaMa.API.Services
{
    public class EstheticianService : IEstheticianService
    {
        private readonly IRepository<Esthetician, int> _estheticians;

        public EstheticianService(IRepository<Esthetician, int> estheticians)
        {
            _estheticians = estheticians;
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
    }

    public interface IEstheticianService
    {
        Task<List<EstheticianViewModel>> GetEstheticiansAsync();
    }
}
