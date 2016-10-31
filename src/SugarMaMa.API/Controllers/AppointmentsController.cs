using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using SugarMaMa.API.Helpers;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Authorization;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.Models.Appointments;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.SpaServices;
using SugarMaMa.API.Services;

namespace SugarMaMa.API.Controllers
{
    [Route("api/[controller]")]
    [ValidateModelState]
    [HandleException]
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly INotificationService _notificationService;
        public AppointmentsController(IAppointmentService appointmentService, INotificationService notificationService)
        {
            _appointmentService = appointmentService;
            _notificationService = notificationService;
        }

        [HttpGet("admin")]
        public async Task<IActionResult> Read()
        {
            var apptsFromDb = await _appointmentService.GetAppointmentsAsync();

            var retval = new List<AppointmentModel>();

            foreach (var appointment in apptsFromDb)
            {
               retval.Add(GenerateApptModel(appointment));
            }
            return Ok(retval);
        }

        [HttpPost("admin")]
        
        public async Task<IActionResult> Create([FromBody]AppointmentModel model)
        {
            var bookingModel = new AppointmentBookingModel
            {
                StartTime = model.Start,
                EndTime = model.End,
                SelectedServices = model.Services,
                SelectedEsthetician = new EstheticianViewModel {  Id = model.EstheticianId },
                NumberOfGuests = 1,
                PhoneNumber = model.Description,
                FirstName = model.FirstName,
                Gender = model.Gender,
                SelectedLocationId = model.LocationId
                
            };
            var result = await _appointmentService.BookAppointmentAsync(bookingModel);
            if (result != null)
            {
                await _notificationService.SendNewAppointmentInfoToEsthetician(result.Id);
            }
            return result != null ? (IActionResult)Ok(GenerateApptModel(result)) : BadRequest("Username already taken.");
        }

        [HttpPut("admin")]
        public async Task<IActionResult> Update([FromBody] AppointmentModel model)
        {
            var updated = await _appointmentService.UpdateAppointmentAsync(model);

            if (updated != null)
                return Ok(GenerateApptModel(updated));
            return new StatusCodeResult(500);
        }

        private AppointmentModel GenerateApptModel(Appointment appointment)
        {
            var servicesSb = new StringBuilder();

            for (var i = 0; i < appointment.Services.Count; i++)
            {
                var svc = appointment.Services[i];
                servicesSb.Append(svc.Name);
                if (i < appointment.Services.Count - 1)
                    servicesSb.Append(", ");
            }

            return new AppointmentModel
            {
                Id = appointment.Id,
                Title = string.Format("{0} - {1}", appointment.FirstName, servicesSb.ToString()),
                FirstName = appointment.FirstName,
                Description = appointment.PhoneNumber,
                EstheticianId = appointment.EstheticianId,
                Services = new List<SpaServiceViewModel>(appointment.Services.Select(x => new SpaServiceViewModel
                {
                    Name = x.Name,
                    Id = x.Id
                })),
                LocationId = appointment.LocationId,
                Start = appointment.StartTime.ToLocalTime(),
                End = appointment.EndTime.ToLocalTime(),
                IsNoShow = appointment.IsNoShow,
                Gender = appointment.Gender
            };
        }
    }

    public class AppointmentModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime Start { get; set; }// = DateTime.SpecifyKind(task.Start, DateTimeKind.Utc),
        public DateTime End { get; set; }// = DateTime.SpecifyKind(task.Start, DateTimeKind.Utc),
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsNoShow { get; set; }
        public bool IsBlockout { get; set; }
        public int EstheticianId { get; set; }
        public int LocationId { get; set; }
        public List<SpaServiceViewModel> Services { get; set; }
        public Gender Gender { get; set; }
    }
}
