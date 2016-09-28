using Microsoft.AspNetCore.Mvc;
using SugarMaMa.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return Ok(services);
        }
    }
}
