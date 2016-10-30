using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;

namespace SugarMaMa.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Appointment> _appointments;

        public NotificationService(ISmsSender smsSender, IRepository<Appointment> appointments)
        {
            _smsSender = smsSender;
            _appointments = appointments;
        }

        public async Task SendNewAppointmentInfoToEsthetician(int apptId)
        {
            var appointment = await _appointments.GetByIdAsync(apptId, x => x.Esthetician, x => x.Esthetician.User,
                x => x.Location);

            var body = "----SugarMaMa New Appointment----" + 
                       "\nDate: " + appointment.StartTime.ToLocalTime().ToString("d") +
                       "\nTime: " + appointment.StartTime.ToLocalTime().ToString("h:mm tt") + " - " + appointment.EndTime.ToLocalTime().ToString("h:mm tt") +
                       "\nLocation: " + appointment.Location.City +
                       "\nClient Name: " + appointment.FirstName +
                       "\nServices: " + string.Join(",", appointment.Services.Select(x => x.Name));

            var to = "+1" + appointment.Esthetician.User.PhoneNumber;

            await Task.FromResult(_smsSender.SendSmsAsync(to, body));
        }
    }

    public interface INotificationService
    {
        Task SendNewAppointmentInfoToEsthetician(int apptId);
    }
}
