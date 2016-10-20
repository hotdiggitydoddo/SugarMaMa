using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SugarMaMa.API.Helpers;
using SugarMaMa.API.Services;

namespace SugarMaMa.API.Controllers
{
    [Route("api/[controller]")]
    [ValidateModelState]
    [HandleException]
    public class LocationsController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly ILogger _logger;
        public LocationsController(ILocationService locationService, ILoggerFactory loggerFactory)
        {
            _locationService = locationService;
            _logger = loggerFactory.CreateLogger<LocationsController>();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _locationService.GetLocationsAsync();
            return Ok(result);
        }
    }
}
