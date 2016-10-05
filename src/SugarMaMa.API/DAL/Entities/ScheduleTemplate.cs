using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Entities
{
    public class ScheduleTemplate : SMEntity
    {
        public int EstheticianId { get; set; }
        public Esthetician Esthetician { get; set; }
        public List<ShiftTemplate> Shifts { get; set; }
    }
}
