using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SugarMaMa.API.DAL;
using SugarMaMa.API.DAL.Entities;

namespace SugarMaMa.API
{
    public static class DbContextExtensions
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<SMDbContext>())
            {
                // perform database delete and create
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                AddLocations(context);
                context.SaveChanges();

                AddBusinessDays(context);
                context.SaveChanges();

                //                AddSpaServices(context);

            }
        }

        private static void AddSpaServices(SMDbContext context)
        {
            throw new NotImplementedException();
        }

        private static void AddBusinessDays(SMDbContext context)
        {
            context.AddRange(
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 9:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 6:30pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Monday, LocationId = 1 },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 7:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Tuesday, LocationId = 1 },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 11:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 5:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Monday, LocationId = 2 }
                );
        }

        private static void AddLocations(SMDbContext context)
        {
            context.AddRange(
                new Location { Address1 = "12362 Beach Blvd - Suite 19", Address2 = "Stanton, CA 90680", BusinessDays = new List<BusinessDay>(), PhoneNumber = "562.484.8653" },
                new Location { Address1 = "451 W. Lambert Rd. - Suite 207", Address2 = "Brea, CA 92821", BusinessDays = new List<BusinessDay>(), PhoneNumber = "562.484.8653" }
                );
        }
    }
}
