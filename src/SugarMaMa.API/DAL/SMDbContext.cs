using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SugarMaMa.API.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace SugarMaMa.API.DAL
{
    public class SMDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
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
