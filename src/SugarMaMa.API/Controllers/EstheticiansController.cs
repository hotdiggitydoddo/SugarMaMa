using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SugarMaMa.API.Services;
using Microsoft.AspNetCore.Authorization;
using SugarMaMa.API.Helpers;
using SugarMaMa.API.Models.Estheticians;

namespace SugarMaMa.API.Controllers
{
    [Route("api/[controller]")]
    [ValidateModelState]
    [HandleException]
    public class EstheticiansController : Controller
    {
        private readonly IEstheticianService _estheticians;
        private readonly IAppointmentService _appointments;
     
        public EstheticiansController(IEstheticianService estheticians, IAppointmentService appointments)
        {
            _estheticians = estheticians;
            _appointments = appointments;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody, Required]AddEstheticianViewModel model)
        {
            var result = await _estheticians.AddEstheticianAsync(model);
            return result != null ? (IActionResult) Ok(result) : BadRequest("Username already taken.");
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                var adminResult = await _estheticians.GetEstheticianMasterListAsync();
                return Ok(adminResult);
            }

            var result = await _estheticians.GetEstheticiansAsync();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _estheticians.GetByIdAsync(id);
            return Ok(result);
        }

        [Route("profile")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (User.Identity.IsAuthenticated && User.HasClaim(x => x.Type == "estheticianId"))
            {
                var adminResult = await _estheticians.GetByIdAsync(int.Parse(User.FindFirst("estheticianId").Value));
                return Ok(adminResult);
            }
            return new UnauthorizedResult();
        }

        [Route("appointments")]
        [Authorize]
        [HttpGet]

        public async Task<IActionResult> Appointments(int id)
        {
            var result = await _appointments.GetAppointmentsByEstheticianId(id);
            foreach (var appointment in result)
            {
                appointment.StartTime = appointment.StartTime.ToLocalTime();
                appointment.EndTime = appointment.EndTime.ToLocalTime();
            }
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _estheticians.GetByEmailAsync(email);
            return Ok(result);
        }

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
