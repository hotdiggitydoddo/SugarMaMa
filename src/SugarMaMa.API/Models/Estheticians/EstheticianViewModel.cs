using System.Collections.Generic;
using SugarMaMa.API.Models.SpaServices;
using SugarMaMa.API.Models.Shifts;

namespace SugarMaMa.API.Models.Estheticians
{
    public class EstheticianViewModel
    {
        public string FirstName { get; set; }
        public List<SpaServiceViewModel> Services { get; set; }
        public List<ShiftViewModel> Shifts { get; set; }
    }
}
