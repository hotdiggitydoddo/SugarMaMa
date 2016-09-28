using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SugarMaMa.API.Models.SpaServices;

namespace SugarMaMa.API.Models.Estheticians
{
    public class EstheticianViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SpaServiceViewModel> Services { get; set; }

    }
}
