using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.Models.BusinessDays
{
    public class BusinessDayViewModel
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public LocationViewModel Location { get; set; }
    }
}
