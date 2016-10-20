using SugarMaMa.API.DAL.Entities;
using SugarMaMa.API.Models.Estheticians;
using SugarMaMa.API.Models.SpaServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SugarMaMa.API.Models.Appointments
{
    public class AppointmentBookingModel
    {
        [Required, Range(1, 3)]
        public int NumberOfGuests { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public List<SpaServiceViewModel> SelectedServices { get; set; }

        [Required]
        public EstheticianViewModel SelectedEsthetician { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required, StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required, StringLength(25, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ReminderViaText { get; set; }
        public bool ReminderViaEmail { get; set; }
        public int SelectedLocationId { get; set; }
    }
}
