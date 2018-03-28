using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarsRentMVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class AppDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Создаем две роли
            var role1 = new IdentityRole {Name = "admin"};
            var role2 = new IdentityRole {Name = "manager"};
            var role3 = new IdentityRole {Name = "master"};
            var role4 = new IdentityRole {Name = "user"};

            // Добавляем роли в БД
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);
            roleManager.Create(role4);

            // Создаем пользователей
            var admin = new ApplicationUser {Email = "Administrator@gmail.com", UserName = "Administrator@gmail.com", NickName = "Administrator"};
            string password1 = "11111a";
            var result1 = userManager.Create(admin, password1);

            var manager = new ApplicationUser {Email = "Manager@gmail.com", UserName = "Manager@gmail.com", NickName = "Manager"};
            string password2 = "11111a";
            var result2 = userManager.Create(manager, password2);

            var master = new ApplicationUser {Email = "Master@gmail.com", UserName = "Master@gmail.com", NickName = "Master"};
            string password3 = "11111a";
            var result3 = userManager.Create(master, password3);

            // Если создание пользователей прошло успешно
            if (result1.Succeeded) {
                // Добавляем роль для пользователей
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
                userManager.AddToRole(admin.Id, role3.Name);
                userManager.AddToRole(admin.Id, role4.Name);
            }

            if (result2.Succeeded) {
                // Добавляем роль для пользователей
                userManager.AddToRole(manager.Id, role2.Name);
                userManager.AddToRole(manager.Id, role4.Name);
            }

            if (result3.Succeeded) {
                // Добавляем роль для пользователей
                userManager.AddToRole(master.Id, role3.Name);
                userManager.AddToRole(master.Id, role4.Name);
            }

            base.Seed(context);
        }
    }
}