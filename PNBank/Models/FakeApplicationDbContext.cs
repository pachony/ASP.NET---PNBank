using System.Data.Entity;

namespace PNBank.Models
{
    public class FakeApplicationDbContext : IApplicationDbContext
    {
        public FakeApplicationDbContext()
        {
            this.CheckingAccounts = new FakeDbSet<CheckingAccount>();
            this.Transactions = new FakeDbSet<Transaction>();
        }
    public IDbSet<CheckingAccount> CheckingAccounts { get; set; }
    public IDbSet<Transaction> Transactions { get; set; }

    public int SaveChanges()
    {
        return 0;
    }
    }
}