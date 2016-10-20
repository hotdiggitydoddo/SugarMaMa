using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;

namespace SugarMaMa.API.DAL.Entities
{
    public class Appointment : SMEntity
    {
        public int EstheticianId { get; set; }
        public Esthetician Esthetician { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public List<SpaService> Services { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool RemindViaText { get; set; }
        public bool RemindViaEmail { get; set; }
        public int? ClientId { get; set; }
        public Client Client { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal Cost { get; set; }
        public Gender Gender { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public bool IsNoShow { get; set; }
    }
}
