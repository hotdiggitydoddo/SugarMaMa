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
     
        public EstheticiansController(IEstheticianService estheticians)
        {
            _estheticians = estheticians;
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
        public string Get(int id)
        {
            return "value";
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
