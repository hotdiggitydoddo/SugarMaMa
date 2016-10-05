using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Entities
{
    public class Shift : SMEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EstheticianId { get; set; }
        public Esthetician Esthetician { get; set; }
        public int BusinessDayId { get; set; }
        public BusinessDay BusinessDay { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
