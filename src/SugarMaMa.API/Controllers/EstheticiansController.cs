using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SugarMaMa.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace SugarMaMa.API.Controllers
{
    [Route("api/[controller]")]
    public class EstheticiansController : Controller
    {
        private readonly IEstheticianService _estheticians;

        public EstheticiansController(IEstheticianService estheticians)
        {
            _estheticians = estheticians;
        }

        [HttpGet]
        [Authorize(Roles = "Esthetician")]
        public async Task<IActionResult> Get()
        {
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
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
