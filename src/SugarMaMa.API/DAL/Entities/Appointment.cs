using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;

namespace SugarMaMa.API.DAL.Entities
{
    public class Appointment : SMEntity
    {
        public List<SpaService> Services { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool RemindViaText { get; set; }
        public bool RemindViaEmail { get; set; }
        public Guid ShiftId { get; set; }
        public Shift Shift { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
    }
}
