using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Helpers;

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

            var start = appointment.StartTime.ConvertToLocalTime("US/Pacific");
            var end = appointment.EndTime.ConvertToLocalTime("US/Pacific");

            var body = "----SugarMaMa New Appointment----" + 
                       "\nDate: " + start.ToString("d") +
                       "\nTime: " + start.ToString("h:mm tt") + " - " + end.ToString("h:mm tt") +
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
