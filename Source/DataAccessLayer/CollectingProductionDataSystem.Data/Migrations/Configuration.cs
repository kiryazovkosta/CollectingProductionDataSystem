namespace CollectingProductionDataSystem.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.Identity;
    using EntityFramework.BulkInsert.Extensions;
    using Microsoft.AspNet.Identity;
    using CollectingProductionDataSystem.Data.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<CollectingDataSystemDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CollectingDataSystemDbContext context)
        {
            //if (!context.Shifts.Any())
            //{
            //    this.CreateShifts(context);
            //}
            //if (!context.Roles.Any())
            //{
            //    this.CreateRoles(context);

            //    //this.SeedUsers(context);
            //}
            //this.CreateSystemAdministrator(context);
        }

        /// <summary>
        /// Creates the system administrator.
        /// </summary>
        /// <param name="context">The context.</param>
        private void CreateSystemAdministrator(CollectingDataSystemDbContext context)
        {
            if (context.Users.Where(x => x.UserName == "Administrator").FirstOrDefault() == null)
            {
                var user = new ApplicationUser()
                {
                    Id = 1,
                    Email = "Nikolay.Kostadinov@bmsys.eu",
                    UserName = "Administrator",
                    CreatedOn = DateTime.Now,
                    CreatedFrom = "InitialLoading",
                    PasswordHash = new PasswordHasher().HashPassword(CommonConstants.StandartPassword),
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                user.Roles.Add(new UserRoleIntPk() { UserId = user.Id, RoleId = 1 });
                context.Users.Add(user);
                //context.SaveChanges("InitialLoading");
                //var userManger = new UserManager<ApplicationUser,int>(new UserStoreIntPk(context));
                //userManger.Create(user, CommonConstants.StandartPassword);
                //userManger.AddToRole(user.Id,"Administrator");
                ////context.SaveChanges("InitialLoading");
            }
        }

        private void CreateShifts(CollectingDataSystemDbContext context)
        {
            context.Shifts.AddOrUpdate(
                p => p.Id,
                new Shift
                {
                    Id = 1,
                    Name = "Първа смяна",
                    BeginTime = new TimeSpan(5, 1, 0),
                    ReadOffset = new TimeSpan(13, 0, 0),
                    ReadPollTimeSlot = new TimeSpan(2, 0, 0)
                },
                new Shift
                {
                    Id = 2,
                    Name = "Втора смяна",
                    BeginTime = new TimeSpan(13, 1, 0),
                    ReadOffset = new TimeSpan(21, 0, 0),
                    ReadPollTimeSlot = new TimeSpan(2, 0, 0)
                },
                new Shift
                {
                    Id = 3,
                    Name = "Трета смяна",
                    BeginTime = new TimeSpan(-2, -59, 0),
                    ReadOffset = new TimeSpan(5, 0, 0),
                    ReadPollTimeSlot = new TimeSpan(2, 0, 0)
                });
        }

        private void CreateRoles(CollectingDataSystemDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<ApplicationRole>()
                {
                    new ApplicationRole{
                        Id = 1,
                        Name = "Administrator"
                    },
                    new ApplicationRole
                    {
                        Id=2,
                        Name = "ShiftReporter",
                        Description = "Изготвяне на сменни отчети"
                    },
                    new ApplicationRole
                    {
                        Id=3,
                        Name = "DailyReporter",
                        Description = "Изгорвяне на дневни отчети"
                    },
                    new ApplicationRole
                    {
                        Id=4,
                        Name= "MonthlyReporter",
                        Description = "За бъдещо развитие на системата"
                    },
                     new ApplicationRole
                    {
                        Id=5,
                        Name= "PowerUser",
                        Description="В комбинация с ShiftReporter и DailyReporter дава достъп до всички инсталации и паркове"
                    },
                    new ApplicationRole
                    {
                        Id=6,
                        Name= "NomManager",
                        Description = "Управление на номенклатури"
                    },
                    new ApplicationRole
                    {
                        Id=6,
                        Name = "HistoryManager",
                        Description = "Достъп до историята на записите в системата"
                    }
                };

                var roleManager = new RoleManager<ApplicationRole, int>(new RoleStoreIntPk(context));

                foreach (var role in roles)
                {
                    roleManager.Create(role);
                }

                context.SaveChanges("InitialLoading");
            }
        }

        private void SeedUsers(CollectingDataSystemDbContext context)
        {
            var users = new List<ApplicationUser>();
            var hasher = new PasswordHasher();
            for (int i = 2; i < 200; i++)
            {
                if (context.Users.Where(x => x.UserName == "User_" + i).FirstOrDefault() == null)
                {
                    var user = new ApplicationUser()
                    {
                        Email = "User_" + i + "@bmsys.eu",
                        UserName = "User_" + i,
                        PasswordHash = hasher.HashPassword(CommonConstants.StandartPassword),
                        SecurityStamp = Guid.NewGuid().ToString(),
                        FirstName = "Name" + i,
                        MiddleName = "Sirname" + i,
                        LastName = "Family" + i,
                        Occupation = "Test Ltd."
                    };

                    users.Add(user);
                }
            }

            var timer = new Stopwatch();
            timer.Start();
            context.BulkInsert(users);
            context.SaveChanges("Initial Loading");
            timer.Stop();
            Debug.WriteLine("Estimated time for loading " + timer.Elapsed);
        }
    }
}