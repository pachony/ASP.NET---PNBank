using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PNBank.Migrations;

namespace PNBank.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = true;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<CheckingAccount> CheckingAccounts { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        //public System.Data.Entity.DbSet<AutomatedTellerMachine.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }

}