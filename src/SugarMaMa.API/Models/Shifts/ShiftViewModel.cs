using SugarMaMa.API.Models.BusinessDays;
using SugarMaMa.API.Models.Estheticians;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.Models.Shifts
{
    public class ShiftViewModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BusinessDayViewModel BusinessDay { get; set; }
    }
}
