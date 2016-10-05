using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Entities
{
    public class Client : SMEntity
    {
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool AppointmentRemindersViaText { get; set; }
        public bool AppointmentRemindersViaEmail { get; set; }
        public int NoShows { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
