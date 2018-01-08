using System.ComponentModel.DataAnnotations;

namespace PNBank.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public int CheckingAccountId { get; set; }
        
        public virtual CheckingAccount CheckingAccount { get; set; }
    }
}