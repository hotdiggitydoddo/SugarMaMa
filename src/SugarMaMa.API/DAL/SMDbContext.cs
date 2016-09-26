using System;
using SugarMaMa.API.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace SugarMaMa.API.DAL
{
    public class SMDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Esthetician> Estheticians { get; set; }
        public DbSet<SpaService> SpaServices { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BusinessDay> BusinessDays { get; set; }

        public SMDbContext(DbContextOptions<SMDbContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("uuid-ossp");
        }

    }
}
