using System;
using System.Collections.Generic;

namespace SugarMaMa.API.DAL.Entities
{
    public class Appointment : SMEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<SpaService> Services { get; set; }
        public int EstheticianId { get; set; }
        public Esthetician Esthetician { get; set; }
        public bool RemindViaText { get; set; }
        public bool RemindViaEmail { get; set; }
    }
}
