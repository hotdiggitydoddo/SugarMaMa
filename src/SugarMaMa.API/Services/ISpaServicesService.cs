using SugarMaMa.API.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.Services
{
    public interface ISpaServicesService
    {
        Task<IEnumerable<SpaService>> GetAllAsync();
    }
}
