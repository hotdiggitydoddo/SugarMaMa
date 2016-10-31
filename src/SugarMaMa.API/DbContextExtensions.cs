using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
                AddShiftsToEstheticians(context);

                AddAppointments(context, userManager);
            }
        }

        private static void AddAppointments(SMDbContext context, UserManager<ApplicationUser> userManager)
        {
            var esth = context.Estheticians.Single(x => x.Id == 1);
            var services = context.SpaServices.Take(3).ToList();
            var clientUser = new ApplicationUser
            {
                UserName = "client@test.com",
                Email = "client@test.com",
                FirstName = "Claire",
                LastName = "Smith",
                PhoneNumber = "5625551212"
            };

            Task.FromResult(userManager.CreateAsync(clientUser, "client11").Result);

            var client = new Client
            {
                ApplicationUserId = userManager.FindByEmailAsync("client@test.com").Result.Id,
                AppointmentRemindersViaText = true,
                Appointments = new List<Appointment>()
            };
            context.Clients.Add(client);
            context.SaveChanges();

            var appt = new Appointment
            {
                ClientId = client.Id,
                FirstName = clientUser.FirstName,
                LastName = clientUser.LastName,
                Email = clientUser.Email,
                PhoneNumber = clientUser.PhoneNumber,
                Services = services,
                Esthetician = esth,
                StartTime = DateTime.Parse("10/21/2016 17:15"),
                LocationId = 1,
                EndTime = DateTime.Parse("10/21/2016 18:30"),
                RemindViaEmail = client.AppointmentRemindersViaEmail,
                RemindViaText = client.AppointmentRemindersViaText,
                Gender = Gender.Male
            };

            context.Appointments.Add(appt);
            context.SaveChanges();
        }

        private static void AddLocations(SMDbContext context)
        {
            context.AddRange(
                new Location
                {
                    Address1 = "12362 Beach Blvd",
                    Address2 = "Suite 19",
                    City = "Stanton",
                    State = "CA",
                    ZipCode = "90680",
                    BusinessDays = new List<BusinessDay>(),
                    PhoneNumber = "562.484.8653"
                },
                new Location
                {
                    Address1 = "451 W. Lambert Rd.",
                    Address2 = "Suite 207",
                    City = "Brea",
                    State = "CA",
                    ZipCode = "92821",
                    BusinessDays = new List<BusinessDay>(),
                    PhoneNumber = "562.484.8653"
                }
                );
        }

        private static void AddBusinessDays(SMDbContext context)
        {
            var locations = context.Locations.ToList();

            locations[0].BusinessDays.AddRange(new[] {
                // Stanton
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Monday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 7:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Tuesday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 12:00pm").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Wednesday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 12:00pm").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Thursday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 09:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Friday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00pm").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 4:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Saturday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 12:00pm").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 5:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Sunday }
            });

            locations[1].BusinessDays.AddRange(new[] {
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Monday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 9:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Tuesday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 9:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Wednesday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 8:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Thursday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 12:00pm").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 6:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Friday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 09:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 4:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Saturday },
                new BusinessDay { OpeningTime = DateTime.Parse("1/1/00 10:00am").ToUniversalTime(), ClosingTime = DateTime.Parse("1/1/00 4:00pm").ToUniversalTime(), DayOfWeek = DayOfWeek.Sunday }
                });
        }

        private static void AddSpaServices(SMDbContext context)
        {
            context.AddRange(
                new SpaService
                {
                    Name = "Eyebrow Shaping",
                    Description = "",
                    ServiceType = SpaServiceTypes.HairRemoval,
                    IsQuickService = true,
                    IsPremium = false,
                    IsUnisex = true,
                    Duration = TimeSpan.FromMinutes(15),
                    Cost = 15
                },
                new SpaService
                {
                    Name = "Lip",
                    Description = "",
                    ServiceType = SpaServiceTypes.HairRemoval,
                    IsQuickService = true,
                    IsPremium = false,
                    IsUnisex = true,
                    Duration = TimeSpan.FromMinutes(10),
                    Cost = 10
                },
                 new SpaService
                 {
                     Name = "Chin",
                     Description = "",
                     ServiceType = SpaServiceTypes.HairRemoval,
                     IsQuickService = true,
                     IsPremium = false,
                     IsUnisex = true,
                     Duration = TimeSpan.FromMinutes(10),
                     Cost = 10
                 },
                  new SpaService
                  {
                      Name = "Nostrils",
                      Description = "",
                      ServiceType = SpaServiceTypes.HairRemoval,
                      IsQuickService = true,
                      IsPremium = false,
                      IsUnisex = true,
                      Duration = TimeSpan.FromMinutes(15),
                      Cost = 10
                  },
                   new SpaService
                   {
                       Name = "Full Face",
                       Description = "Includes brows, nostrils, lip, chin, sideburns",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = true,
                       Duration = TimeSpan.FromMinutes(30),
                       Cost = 40
                   },
                   new SpaService
                   {
                       Name = "Underarms",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = true,
                       Duration = TimeSpan.FromMinutes(15),
                       Cost = 15
                   },
                   new SpaService
                   {
                       Name = "Arms",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = true,
                       Duration = TimeSpan.FromMinutes(15),
                       Cost = 35
                   },
                   new SpaService
                   {
                       Name = "Half Leg",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = true,
                       Duration = TimeSpan.FromMinutes(30),
                       Cost = 50
                   },
                   new SpaService
                   {
                       Name = "Full Leg",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = true,
                       Duration = TimeSpan.FromMinutes(45),
                       Cost = 70
                   },
                   new SpaService
                   {
                       Name = "Bikini",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = false,
                       Duration = TimeSpan.FromMinutes(15),
                       Cost = 35
                   },
                   new SpaService
                   {
                       Name = "Deep Bikini",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = false,
                       Duration = TimeSpan.FromMinutes(15),
                       Cost = 45
                   },
                   new SpaService
                   {
                       Name = "Brazlian",
                       Description = "",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = false,
                       Duration = TimeSpan.FromMinutes(30),
                       Cost = 60
                   },
                   new SpaService
                   {
                       Name = "Chest",
                       Description = "Includes stomach",
                       ServiceType = SpaServiceTypes.HairRemoval,
                       IsQuickService = false,
                       IsPremium = false,
                       IsUnisex = true,
                       Duration = TimeSpan.FromMinutes(30),
                       Cost = 75
                   },
                    new SpaService
                    {
                        Name = "Lower Back",
                        Description = "",
                        ServiceType = SpaServiceTypes.HairRemoval,
                        IsQuickService = false,
                        IsPremium = false,
                        IsUnisex = true,
                        Duration = TimeSpan.FromMinutes(15),
                        Cost = 30
                    },
                     new SpaService
                     {
                         Name = "Full Back",
                         Description = "Includes shoulders",
                         ServiceType = SpaServiceTypes.HairRemoval,
                         IsQuickService = false,
                         IsPremium = false,
                         IsUnisex = true,
                         Duration = TimeSpan.FromMinutes(30),
                         Cost = 55
                     },
                      new SpaService
                      {
                          Name = "The Brazilian Dream",
                          Description = "This is a no-rush, calm and relaxing brazilian. take the edge off knowing you will arrive and enjoy a blend of calming tea while your numbing gel takes effect. Lay back while your brazilian is performed in a calm environment designed to ease all your worries. Finish with a calming mask for your delicate lady parts while enjoying a 10 minute scalp massage and aroma therapy.",
                          ServiceType = SpaServiceTypes.HairRemoval,
                          IsQuickService = false,
                          IsPremium = true,
                          IsUnisex = false,
                          Duration = TimeSpan.FromMinutes(45),
                          Cost = 75
                      },
                       new SpaService
                       {
                           Name = "The Bold and the Beautiful",
                           Description = "This service includes a Brazilian sugaring and a 60 min. customized facial. Nobody looks forward to having their pubic hair removed, so why not couple it with some pleasure and walk out feeling beautiful and relaxed from head to toe!",
                           ServiceType = SpaServiceTypes.HairRemoval,
                           IsQuickService = false,
                           IsPremium = true,
                           IsUnisex = false,
                           Duration = TimeSpan.FromMinutes(90),
                           Cost = 99
                       },
                        new SpaService
                        {
                            Name = "Sideburns",
                            Description = "",
                            ServiceType = SpaServiceTypes.HairRemoval,
                            IsQuickService = true,
                            IsPremium = false,
                            IsUnisex = true,
                            Duration = TimeSpan.FromMinutes(15),
                            Cost = 15
                        },
                          new SpaService
                          {
                              Name = "Tummy",
                              Description = "",
                              ServiceType = SpaServiceTypes.HairRemoval,
                              IsQuickService = false,
                              IsPremium = false,
                              IsUnisex = true,
                              Duration = TimeSpan.FromMinutes(15),
                              Cost = 20
                          },
                         new SpaService
                         {
                             Name = "Tush",
                             Description = "Butt cheeks",
                             ServiceType = SpaServiceTypes.HairRemoval,
                             IsQuickService = false,
                             IsPremium = false,
                             IsUnisex = true,
                             Duration = TimeSpan.FromMinutes(15),
                             Cost = 25
                         },
                        new SpaService
                        {
                            Name = "Single Tan",
                            Description = "",
                            ServiceType = SpaServiceTypes.Tanning,
                            IsQuickService = false,
                            IsPremium = false,
                            IsUnisex = true,
                            Duration = TimeSpan.FromMinutes(15),
                            Cost = 35
                        },
                         new SpaService
                         {
                             Name = "The Petit",
                             Description = "A 30 minute facial that includes an ultrasonic cleanse, exfoliation, and mask. This treatment is perfect for a \"pick me up\" in between stronger treatments or before a big event.",
                             ServiceType = SpaServiceTypes.Facial,
                             IsQuickService = false,
                             IsPremium = false,
                             IsUnisex = true,
                             Duration = TimeSpan.FromMinutes(30),
                             Cost = 45
                         },
                        new SpaService
                        {
                            Name = "The Grand",
                            Description = "A 60 minute facial that includes an ultrasonic cleanse, exfoliation, extractions, hot stone massage, and mask. This treatment is designed to deliver maximum results for clean, glowing skin, as well as relaxation.",
                            ServiceType = SpaServiceTypes.Facial,
                            IsQuickService = false,
                            IsPremium = false,
                            IsUnisex = true,
                            Duration = TimeSpan.FromMinutes(70),
                            Cost = 60
                        },
                          new SpaService
                          {
                              Name = "The Luxe",
                              Description = "This 80 minute facial is our most luxurious. It is designed to induce relaxation, as well as treat all your skin care needs. This treatment begins face-down with a hot stone massage and includes an ultrasonic cleanse, exfoliation, extractions, eye treatment, and galvanic current.",
                              ServiceType = SpaServiceTypes.Facial,
                              IsQuickService = false,
                              IsPremium = false,
                              IsUnisex = true,
                              Duration = TimeSpan.FromMinutes(80),
                              Cost = 110
                          },
                           new SpaService
                           {
                               Name = "Crystal-Free Diamond-Tip Microdermabrasion",
                               Description = "This treatment is designed to brighten the skin, leaving it glowing, radiant, and smooth. It can also help soften the look of fine lines. A diamond-tip exfoliation is performed. This is a great anti-aging treatment with no downtime. Not recommended for acne or severe rosacea.",
                               ServiceType = SpaServiceTypes.Microderm,
                               IsQuickService = false,
                               IsPremium = false,
                               IsUnisex = true,
                               Duration = TimeSpan.FromMinutes(60),
                               Cost = 90
                           },
                           new SpaService
                           {
                               Name = "Micro Express",
                               Description = "This 30 minute treatment is desgined for anyone on the go looking for rapid results. Treatment includes double-cleanse, 2 passes with diamond tip, and mask.",
                               ServiceType = SpaServiceTypes.Microderm,
                               IsQuickService = false,
                               IsPremium = false,
                               IsUnisex = true,
                               Duration = TimeSpan.FromMinutes(30),
                               Cost = 45
                           },
                            new SpaService
                            {
                                Name = "Lactic Acid",
                                Description = "Helps to soften, stimulate, smooth, and exfoliate the skin. Perfect before a big event.  (no peeling)",
                                ServiceType = SpaServiceTypes.ChemicalPeel,
                                IsQuickService = false,
                                IsPremium = false,
                                IsUnisex = true,
                                Duration = TimeSpan.FromMinutes(30),
                                Cost = 55
                            },
                              new SpaService
                              {
                                  Name = "Glycolic Acid",
                                  Description = "A workout for your skin. Glycolic is known for its tightening affects.  (no peeling)",
                                  ServiceType = SpaServiceTypes.ChemicalPeel,
                                  IsQuickService = false,
                                  IsPremium = false,
                                  IsUnisex = true,
                                  Duration = TimeSpan.FromMinutes(30),
                                  Cost = 60
                              },

                                 new SpaService
                                 {
                                     Name = "Salycilic Acid",
                                     Description = "Helps to dissolve build-up on skin, kill acne-causing bacteria, as well as lighten hyperpigmentation.  (expect peeling for up to 3-10 days)",
                                     ServiceType = SpaServiceTypes.ChemicalPeel,
                                     IsQuickService = false,
                                     IsPremium = false,
                                     IsUnisex = true,
                                     Duration = TimeSpan.FromMinutes(30),
                                     Cost = 60
                                 },
                                  new SpaService
                                  {
                                      Name = "Tca",
                                      Description = "Helps to exfoliate dead skin cells, reduces the appearance of fine lines and wrinkles. Tightens, tones, and smoothes skin texture.  (expect peeling for up to 5-7 days)",
                                      ServiceType = SpaServiceTypes.ChemicalPeel,
                                      IsQuickService = false,
                                      IsPremium = false,
                                      IsUnisex = true,
                                      Duration = TimeSpan.FromMinutes(30),
                                      Cost = 80
                                  },
                                   new SpaService
                                   {
                                       Name = "Jessner",
                                       Description = "The big guns... This peel is designed to retexturize skin, smooth scarring, and reduce hyperpigmentation.  (expect peeling and flaking for 7-10 days)",
                                       ServiceType = SpaServiceTypes.ChemicalPeel,
                                       IsQuickService = false,
                                       IsPremium = false,
                                       IsUnisex = true,
                                       Duration = TimeSpan.FromMinutes(30),
                                       Cost = 80
                                   },
                                 new SpaService
                                 {
                                     Name = "Eyebrow Tinting",
                                     Description = "",
                                     ServiceType = SpaServiceTypes.Tinting,
                                     IsQuickService = false,
                                     IsPremium = false,
                                     IsUnisex = true,
                                     Duration = TimeSpan.FromMinutes(15),
                                     Cost = 15
                                 },
                                  new SpaService
                                  {
                                      Name = "Eyelash Tinting",
                                      Description = "",
                                      ServiceType = SpaServiceTypes.Tinting,
                                      IsQuickService = false,
                                      IsPremium = false,
                                      IsUnisex = true,
                                      Duration = TimeSpan.FromMinutes(15),
                                      Cost = 20
                                  }
                );
        }

        private static void AddUsers(UserManager<ApplicationUser> users)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                FirstName = "Alice",
                LastName = "Smith",
                PhoneNumber = "6265551212"
            };

            var estheticianUser = new ApplicationUser
            {
                UserName = "eve@test.com",
                Email = "eve@test.com",
                FirstName = "Eve",
                LastName = "Smith",
                PhoneNumber = "7144729124"
            };

            Task.FromResult(users.CreateAsync(adminUser, "admin11").Result);
            Task.FromResult(users.CreateAsync(estheticianUser, "regular11").Result);
        }

        private static void AddRoles(RoleManager<ApplicationRole> roleManager)
        {
            Task.FromResult(roleManager.CreateAsync(new ApplicationRole { Name = "Admin" }).Result);
            Task.FromResult(roleManager.CreateAsync(new ApplicationRole { Name = "Esthetician" }).Result);
            Task.FromResult(roleManager.CreateAsync(new ApplicationRole { Name = "Client" }).Result);
        }

        private static void AddUsersToRoles(UserManager<ApplicationUser> users, RoleManager<ApplicationRole> roleManager)
        {
            var admin = users.FindByEmailAsync("admin@test.com").Result;
            var esthetician = users.FindByEmailAsync("eve@test.com").Result;

            Task.FromResult(users.AddToRolesAsync(admin, new[] { "Admin", "Esthetician" }).Result);
            Task.FromResult(users.AddToRoleAsync(esthetician, "Esthetician").Result);
        }

        private static void AddEstheticians(UserManager<ApplicationUser> users, RoleManager<ApplicationRole> roles, SMDbContext context)
        {
            var esthUser = users.FindByEmailAsync("eve@test.com").Result;
            var allServices = context.SpaServices.ToList();

            var esthetician = new Esthetician
            {
                ApplicationUserId = esthUser.Id,
                Services = allServices.OrderBy(x => x.Id).Take(12).ToList(),
                Color = "33cc33"
            };

            context.Estheticians.Add(esthetician);

            var nextUser = users.FindByEmailAsync("admin@test.com").Result;
            var nextEsth = new Esthetician
            {
                ApplicationUserId = nextUser.Id,
                Services = allServices.ToList(),
                Color = "cc33ff"
            };

            context.Estheticians.Add(nextEsth);

            context.SaveChanges();
        }

        private static void AddShiftsToEstheticians(SMDbContext context)
        {
            var estheticians = context.Estheticians.ToList();
            var businessDays = context.BusinessDays.ToList();

            var esth = estheticians.First();

            var shifts = new List<Shift>
            {
                new Shift
                {
                    Esthetician = esth,
                    BusinessDay = businessDays.Single(x => x.DayOfWeek == DayOfWeek.Monday && x.LocationId == 2),
                    StartTime = DateTime.Parse("1/1/00 10:00am"),
                    EndTime = DateTime.Parse("1/1/00 4:00pm")
                },
                new Shift
                {
                    Esthetician = esth,
                    BusinessDay = businessDays.Single(x => x.DayOfWeek == DayOfWeek.Tuesday && x.LocationId == 1),
                    StartTime = DateTime.Parse("1/1/00 10:00am"),
                    EndTime = DateTime.Parse("1/1/00 7:00pm")
                },
                new Shift
                {
                    Esthetician = esth,
                    BusinessDay = businessDays.Single(x => x.DayOfWeek == DayOfWeek.Wednesday && x.LocationId == 1),
                    StartTime = DateTime.Parse("1/1/00 12:00pm"),
                    EndTime = DateTime.Parse("1/1/00 8:00pm")
                }
            };
            esth.Shifts = new List<Shift>();
            esth.Shifts.AddRange(shifts);

            var nextEsth = estheticians.Last();

            var nextShifts = new List<Shift>
            {
                new Shift
                {
                    Esthetician = nextEsth,
                    BusinessDay = businessDays.Single(x => x.DayOfWeek == DayOfWeek.Monday && x.LocationId == 1),
                    StartTime = DateTime.Parse("1/1/00 4:00pm"),
                    EndTime = DateTime.Parse("1/1/00 8:00pm")
                },
                new Shift
                {
                    Esthetician = nextEsth,
                    BusinessDay = businessDays.Single(x => x.DayOfWeek == DayOfWeek.Thursday && x.LocationId == 2),
                    StartTime = DateTime.Parse("1/1/00 10:00am"),
                    EndTime = DateTime.Parse("1/1/00 8:00pm")
                },
                new Shift
                {
                    Esthetician = nextEsth,
                    BusinessDay = businessDays.Single(x => x.DayOfWeek == DayOfWeek.Friday && x.LocationId == 2),
                    StartTime = DateTime.Parse("1/1/00 12:00pm"),
                    EndTime = DateTime.Parse("1/1/00 6:00pm")
                }
            };

            nextEsth.Shifts = new List<Shift>();
            nextEsth.Shifts.AddRange(nextShifts);

            context.SaveChanges();
        }
    }
}
