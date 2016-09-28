using Microsoft.AspNetCore.Mvc;
using SugarMaMa.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SugarMaMa.API.DAL.Entities;

namespace SugarMaMa.API.Controllers
{
    [Route("api/[controller]")]
    public class SpaServicesController : Controller
    {
        private readonly ISpaServicesService _spaService;

        public SpaServicesController(ISpaServicesService spaService)
        {
            _spaService = spaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var services = await _spaService.GetAllAsync();
            var spaServices = services as SpaService[] ?? services.ToArray();

            var hairRemoval = spaServices.Where(x => x.ServiceType == SpaServiceTypes.HairRemoval);
            var facials = spaServices.Where(x => x.ServiceType == SpaServiceTypes.Facial);
            var tanning = spaServices.Where(x => x.ServiceType == SpaServiceTypes.Tanning);
            var tinting = spaServices.Where(x => x.ServiceType == SpaServiceTypes.Tinting);
            var peels = spaServices.Where(x => x.ServiceType == SpaServiceTypes.ChemicalPeel);
            var microderm = spaServices.Where(x => x.ServiceType == SpaServiceTypes.Microderm);

            return Ok(new { hairRemoval, facials, tanning, tinting, peels, microderm });
        }
    }
}
