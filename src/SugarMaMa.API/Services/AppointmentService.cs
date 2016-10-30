using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SugarMaMa.API.Controllers;
using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.DAL.Repositories;
using SugarMaMa.API.Models.Appointments;

namespace SugarMaMa.API.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointments;
        private readonly IRepository<Esthetician> _estheticians;
        private readonly IRepository<SpaService> _spaServices;
        private readonly IRepository<Shift> _shifts;

        private readonly UserManager<ApplicationUser> _users;

        public AppointmentService(IRepository<Appointment> appointmentRepository, UserManager<ApplicationUser> users,
            IRepository<Esthetician> estheticians, IRepository<SpaService> spaServices, IRepository<Shift> shifts)
        {
            _appointments = appointmentRepository;
            _users = users;
            _estheticians = estheticians;
            _spaServices = spaServices;
            _shifts = shifts;
        }

        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            var appointments =
                await _appointments.GetAsync(x => x.Client, x => x.Esthetician, x => x.Services, x => x.Location);
            return appointments.ToList();
        }

        public async Task<Appointment> BookAppointmentAsync(AppointmentBookingModel model)
        {
            var esth = _estheticians.GetByIdAsync(model.SelectedEsthetician.Id, x => x.User);
            var services = await _spaServices.FindAsync(x => model.SelectedServices.Select(o => o.Id).Contains(x.Id));
            Client client = null;

            //Create the user if they added a password
            if (model.Password != null)
            {
                var clientUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };
                await Task.FromResult(_users.CreateAsync(clientUser, model.Password).Result);

                client = new Client
                {
                    ApplicationUserId = _users.FindByEmailAsync(model.Email).Result.Id,
                    AppointmentRemindersViaText = model.ReminderViaText,
                    AppointmentRemindersViaEmail = model.ReminderViaEmail,
                    Appointments = new List<Appointment>()
                };

            }

            var appt = new Appointment
            {
                Services = services.ToList(),
                Esthetician = await esth,
                StartTime = model.StartTime.ToUniversalTime(),
                EndTime = model.EndTime.ToUniversalTime(),
                RemindViaEmail = model.ReminderViaEmail,
                RemindViaText = model.ReminderViaText,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                LocationId = model.SelectedLocationId,
                Gender = model.Gender
            };

            //appt.Cost = appt.Services.Sum(x => x.Cost);

            //link the created user, if any
            if (client != null)
            {
                appt.Client = client;
            }
            try
            {
                return await _appointments.AddAsync(appt);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public async Task<Appointment> UpdateAppointmentAsync(AppointmentModel model)
        {
            
            var existing = await _appointments.GetByIdAsync(model.Id, x => x.Services);

            existing.EndTime = model.End.ToUniversalTime();
            existing.StartTime = model.Start.ToUniversalTime();
            existing.IsNoShow = model.IsNoShow;
            existing.FirstName = model.FirstName;
            existing.PhoneNumber = model.Description;
            existing.Gender = model.Gender;

            if (model.EstheticianId != existing.EstheticianId)
            {
                var newEsth = await _estheticians.GetByIdAsync(model.EstheticianId);

                //existing.Esthetician = newEsth;
                existing.EstheticianId = newEsth.Id;
                existing.Esthetician = null;
            }

            var newServices = await _spaServices.FindAsync(x => model.Services.Select(o => o.Id).Contains(x.Id));
            existing.Services = newServices.ToList();

            if (model.LocationId != existing.LocationId)
            {
                existing.LocationId = model.LocationId;
                existing.Location = null;
            }

            return await _appointments.UpdateAsync(existing) ? existing : null;
        }

        public async Task<List<Appointment>> GetAppointmentsByEstheticianId(int estheticianId, bool all = false)
        {
            var appts = all
                ? await _appointments.FindAsync(x => x.EstheticianId == estheticianId, x => x.Services, x => x.Location)
                : await
                    _appointments.FindAsync(x => x.EstheticianId == estheticianId && x.StartTime > DateTime.UtcNow,
                        x => x.Services, x => x.Location);
            return appts.ToList();
        }
    }

    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAppointmentsAsync();
        Task<Appointment> BookAppointmentAsync(AppointmentBookingModel model);
        Task<Appointment> UpdateAppointmentAsync(AppointmentModel model);
        Task<List<Appointment>> GetAppointmentsByEstheticianId(int estheticianId, bool all = false);
    }
}
