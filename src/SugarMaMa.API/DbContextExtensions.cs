using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
            var userManager = app.ApplicationServices.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<ApplicationRole>>();

            using (var context = app.ApplicationServices.GetRequiredService<SMDbContext>())
            {
                // perform database delete and create
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                AddLocations(context);
                context.SaveChanges();

                AddBusinessDays(context);
                context.SaveChanges();

                AddSpaServices(context);
                context.SaveChanges();

                AddRoles(roleManager);
                AddUsers(userManager);
                AddUsersToRoles(userManager, roleManager);

                AddEstheticians(userManager, roleManager, context);
            }
        }

        private static void AddEstheticians(UserManager<ApplicationUser> users, RoleManager<ApplicationRole> roles, SMDbContext context)
        {
            var esthUser = users.FindByEmailAsync("eve@test.com").Result;
            var allServices = context.SpaServices.ToList();

            var esthetician = new Esthetician
            {
                ApplicationUserId = esthUser.Id,
                Services = allServices
            };

            context.Estheticians.Add(esthetician);
            context.SaveChanges();
        }

        private static void AddRoles(RoleManager<ApplicationRole> roleManager)
        {
            Task.FromResult(roleManager.CreateAsync(new ApplicationRole {Name = "Admin"}).Result);
            Task.FromResult(roleManager.CreateAsync(new ApplicationRole { Name = "Esthetician" }).Result);
            Task.FromResult(roleManager.CreateAsync(new ApplicationRole { Name = "Client" }).Result);
        }

        private static void AddUsers(UserManager<ApplicationUser> users)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                FirstName = "Alice",
                LastName = "Smith"
            };
            
            var estheticianUser = new ApplicationUser
            {
                UserName = "eve@test.com",
                Email = "eve@test.com",
                FirstName = "Eve",
                LastName = "Smith",
            };

            Task.FromResult(users.CreateAsync(adminUser, "admin11").Result);
            Task.FromResult(users.CreateAsync(estheticianUser, "regular11").Result);
        }

        private static void AddUsersToRoles(UserManager<ApplicationUser> users, RoleManager<ApplicationRole> roleManager)
        {
            var admin = users.FindByEmailAsync("admin@test.com").Result;
            var esthetician = users.FindByEmailAsync("eve@test.com").Result;

            Task.FromResult(users.AddToRolesAsync(admin, new[] {"Admin", "Esthetician"}).Result);
            Task.FromResult(users.AddToRoleAsync(esthetician, "Esthetician").Result);
        }

        private static void AddSpaServices(SMDbContext context)
        {
            context.AddRange(
                new SpaService { Name = "Lip", Cost = 10M, Description = "Lips to kiss", ServiceType = SpaServiceTypes.HairRemoval, IsQuickService = true, Duration = TimeSpan.FromMinutes(15) }
                );
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

        //private static void Add
    }
}
