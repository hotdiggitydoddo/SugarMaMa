using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SugarMaMa.API.DAL.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       // public Gender Gender { get; set; }
    }

    //public enum Gender
    //{
    //    Female,
    //    Male
    //}
}
