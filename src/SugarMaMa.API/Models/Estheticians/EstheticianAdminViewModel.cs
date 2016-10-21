using SugarMaMa.API.Models.SpaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SugarMaMa.API.Models.Shifts;

namespace SugarMaMa.API.Models.Estheticians
{
    public class EstheticianAdminViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }
        public List<SpaServiceViewModel> Services { get; set; }
        public List<ShiftViewModel> Shifts { get; set; }
    }
}
