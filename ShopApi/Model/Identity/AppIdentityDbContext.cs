using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


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
                    Name = X01Roles.Admin,            //"admin",
                    NormalizedName = X01Roles.Admin
                },
                new IdentityRole
                {
                    Id = USER_ID,
                    Name = X01Roles.Shopper,          //"shopper",
                    NormalizedName = X01Roles.Shopper //покупатель

                },
                 new IdentityRole
                 {
                     Id = MANAGER_Id,
                     Name = X01Roles.Manager,           //"manager",
                     NormalizedName = X01Roles.Manager
                 }

            );
            #endregion


            #region Craete manager and admin from the spa-client

            var passHash = new PasswordHasher<UserIdentityX01>();

            
            var spaClients = Configuration.GetSection("IdentityX01:DbContext:SpaClients").GetChildren().ToList();

            Console.WriteLine("Configuration count-" + spaClients.Count);
            if (spaClients != null)
            {
                foreach (var item in spaClients)
                {
                    var spaclient = Configuration.GetSection("IdentityX01:DbContext:SpaClients:" + item.Key).Get<SpaOption>();
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
                   
                   
                    builder.Entity<UserIdentityX01>().HasData(user_admin);

                     builder.Entity<UserIdentityX01>().HasData(user_manager);

                    //  Console.WriteLine(item.Key);


                    //  Console.WriteLine(spaclient!.Admin.Pass);


                    builder.Entity<IdentityUserRole<string>>().HasData(

                     new IdentityUserRole<string>[] {

                          new() {
                                         RoleId = ADMIN_ID,
                                         UserId = guid_admin

                                },
                           new() {
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
