using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Xml.Linq;

namespace ShopApi.Model.Identity
{
    public class Option
    {
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string Pass { get; set; } = String.Empty;
    }

    public class SpaOption
    {
        public required Option Admin { get; set; }
        public required Option Manager { get; set; }
    }


    public class AppIdentityDbContext : IdentityDbContext<UserIdentityX01>
    {

        private readonly IConfiguration Configuration;
        public AppIdentityDbContext(
            DbContextOptions<AppIdentityDbContext> options,
            IConfiguration configuration

            )
           : base(options)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region  create roles
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";

            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
            const string MANAGER_Id = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";


            // const string ROLE_ID = ADMIN_ID;

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ADMIN_ID,
                    Name = Enum.GetName(typeof(Role), Role.Admin),            //"admin",
                    NormalizedName = Enum.GetName(typeof(Role), Role.Admin)
                },
                new IdentityRole
                {
                    Id = USER_ID,
                    Name = Enum.GetName(typeof(Role), Role.Shopper),          //"shopper",
                    NormalizedName = Enum.GetName(typeof(Role), Role.Shopper) //покупатель

                },
                 new IdentityRole
                 {
                     Id = MANAGER_Id,
                     Name = Enum.GetName(typeof(Role), Role.Manager),           //"manager",
                     NormalizedName = Enum.GetName(typeof(Role), Role.Manager)
                 }

            );
            #endregion


            #region Craete manager and admin from the spa-client

            var passHash = new PasswordHasher<UserIdentityX01>();


            var spaClients = Configuration.GetSection("DbContext:SpaClients").GetChildren().ToList();
            if (spaClients != null)
            {
                foreach (var item in spaClients)
                {
                    var spaclient = Configuration.GetSection("DbContext:SpaClients:" + item.Key).Get<SpaOption>();
                    var guid_admin = Guid.NewGuid().ToString();
                    var guid_manager = Guid.NewGuid().ToString();

                    var user_admin = new UserIdentityX01
                    {

                        Id = guid_admin,
                        UserName = "admin-" + item.Key,
                        NormalizedUserName = "admin-" + item.Key,
                        FirstName = "admin-" + item.Key,
                        LastName = String.Empty,
                        SpaId = item.Key,
                        Email = spaclient!.Admin.Email,
                        NormalizedEmail = spaclient!.Admin.Email.ToUpper(),
                        EmailConfirmed = true,
                        PhoneNumber = spaclient!.Admin.Phone,
                        SecurityStamp = string.Empty

                    };
                    var user_manager = new UserIdentityX01
                    {
                        Id = guid_manager,
                        UserName = "manager-" + item.Key,
                        NormalizedUserName = "manager-" + item.Key,
                        FirstName = "manager-" + item.Key,
                        LastName = String.Empty,
                        SpaId = item.Key,
                        Email = spaclient.Manager.Email,
                        NormalizedEmail = spaclient.Manager.Email.ToUpper(),
                        EmailConfirmed = true,
                        //  PasswordHash = passHash.HashPassword(null, spaclient.Manager.Pass),
                        PhoneNumber = spaclient!.Manager.Phone,
                        SecurityStamp = string.Empty
                    };

                    user_admin.PasswordHash = passHash.HashPassword(user_admin, spaclient.Admin.Pass);
                    user_manager.PasswordHash = passHash.HashPassword(user_manager, spaclient.Manager.Pass);
                    builder.Entity<UserIdentityX01>().HasData(user_admin, user_manager);

                    //  Console.WriteLine(item.Key);


                    //  Console.WriteLine(spaclient!.Admin.Pass);


                    builder.Entity<IdentityUserRole<string>>().HasData(

                 new IdentityUserRole<string>[] {

                new IdentityUserRole<string>
            {
                RoleId = ADMIN_ID,
                UserId = guid_admin

            },
              new IdentityUserRole<string>
            {
                RoleId = MANAGER_Id,
                UserId = guid_manager

            }

                 }
             );


                }
            }
            #endregion









        }


    }
}
