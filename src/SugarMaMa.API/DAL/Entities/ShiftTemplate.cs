using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Entities
{
    public class ShiftTemplate : SMEntity<int>
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ScheduleTemplateId { get; set; }
        public ScheduleTemplate Schedule { get; set; }
        public int BusinessDayId { get; set; }
        public BusinessDay BusinessDay { get; set; }
    }

    public enum ShiftType
    {
        Workday = 0,
        DayOff = 1,
        Vacation = 2,
        SickDay = 3,
        PersonalDay = 4
    }
}
