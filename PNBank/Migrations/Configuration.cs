using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PNBank.Models;
using PNBank.Services;

namespace PNBank.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AutomatedTellerMachine.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var user = new ApplicationUser { UserName = "admin", Email = "admin@mvcatm.com" };
                IdentityResult result = userManager.Create(user, "passW0rd!");

                var service = new CheckingAccountService(context);
                service.CreateCheckingAccount("admin", "user", user.Id, 1000);

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");
                context.SaveChanges();
            }

            //context.Transactions.Add(new Transaction { Amount = 75, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = -25, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 100000, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 19.99m, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 64.40m, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 100, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = -300, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 255.75m, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 198, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 2, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 50, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = -1.50m, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 6100, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = 164.84m, CheckingAccountId = 15 });
            //context.Transactions.Add(new Transaction { Amount = .01m, CheckingAccountId = 15 });


        }

    }
}
