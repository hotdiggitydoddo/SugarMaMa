using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.Services
{
    public class SpaServicesService : ISpaServicesService
    {
        private readonly IRepository<SpaService, int> _servicesRepository;

        public SpaServicesService(IRepository<SpaService, int> servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public Task<IEnumerable<SpaService>> GetAllAsync()
        {
            return _servicesRepository.GetAsync();
        }
    }
}
