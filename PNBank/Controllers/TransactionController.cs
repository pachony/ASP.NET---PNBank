using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PNBank.Models;
using PNBank.Services;

namespace PNBank.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private IApplicationDbContext db;

        public TransactionController()
        {
            db = new ApplicationDbContext();
        }
        public TransactionController(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public ActionResult Deposit(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Find(checkingAccountId);
            var userId = User.Identity.GetUserId();

            if (checkingAccount == null || checkingAccount.ApplicationUserId != User.Identity.GetUserId())

                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Index", "Home");
            }

            return View();


        }


        public ActionResult Withdrawal(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Find(checkingAccountId);
            var userId = User.Identity.GetUserId();

            if (checkingAccount == null || checkingAccount.ApplicationUserId != User.Identity.GetUserId())
            
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(Transaction transaction)
        {
            var checkingAccount = db.CheckingAccounts.Find(transaction.CheckingAccountId);
            if (checkingAccount.Balance < transaction.Amount)
            {
                ModelState.AddModelError("Amount", "You have insufficient funds!");
            }

            if (ModelState.IsValid)
            {
                transaction.Amount = -transaction.Amount;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        public ActionResult Transfer(int checkingAccountId)
        {
            var checkingAccount = db.CheckingAccounts.Find(checkingAccountId);
            var userId = User.Identity.GetUserId();

            if (checkingAccount == null || checkingAccount.ApplicationUserId != User.Identity.GetUserId())

                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(TransferViewModel transfer)
        {
            // check for available funds

            var sourceCheckingAccount = db.CheckingAccounts.Find(transfer.CheckingAccountId);
            if (sourceCheckingAccount.Balance < transfer.Amount)
            {
                ModelState.AddModelError("Amount", "You have insufficient funds!");
            }

            // check for a valid destination account
            var destinationCheckingAccount = db.CheckingAccounts
                .Where(c => c.AccountNumber == transfer.DestinationCheckingAccountNumber).FirstOrDefault();
            if (destinationCheckingAccount == null)
            {
                ModelState.AddModelError("DestinationCheckingAccountNumber", "Invalid destination account number.");
            }

            // add debit/credit transactions and update account balances
            if (ModelState.IsValid)
            {
                db.Transactions.Add(new Transaction
                {
                    CheckingAccountId = transfer.CheckingAccountId,
                    Amount = -transfer.Amount
                });
                db.Transactions.Add(new Transaction
                {
                    CheckingAccountId = destinationCheckingAccount.Id,
                    Amount = transfer.Amount
                });
                db.SaveChanges();

                var service = new CheckingAccountService(db);
                service.UpdateBalance(transfer.CheckingAccountId);
                service.UpdateBalance(destinationCheckingAccount.Id);

                return PartialView("_TransferSuccess", transfer);
            }


            return PartialView("_TransferForm");
        }
    }
}