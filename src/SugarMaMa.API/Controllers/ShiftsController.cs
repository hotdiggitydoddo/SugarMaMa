using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SugarMaMa.API.Helpers;
using SugarMaMa.API.Services;

namespace SugarMaMa.API.Controllers
{
    [Route("api/[controller]")]
    [ValidateModelState]
    [HandleException]
    public class ShiftsController : Controller
    {
        private readonly IEstheticianService _estheticians;

        public ShiftsController(IEstheticianService estheticians)
        {
            _estheticians = estheticians;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _estheticians.GetShiftById(id);
            return Ok(result);
        }

        [HttpGet("esthetician/{id}")]
        public async Task<IActionResult> GetByEsthetician(int id)
        {
            var result = await _estheticians.GetShiftsByEstheticianId(id);
            return Ok(result);
        }
    }
}
