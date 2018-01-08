using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PNBank.Models;

namespace PNBank.Controllers
{
    [Authorize]
    public class CheckingAccountController : Controller
    {

        // GET: CheckingAccount/Details
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var checkingAccount = db.CheckingAccounts.Find(id);
                var userId = User.Identity.GetUserId();
                if(User.IsInRole("Admin"))
                    return View(checkingAccount);
                if (checkingAccount == null || checkingAccount.ApplicationUserId != User.Identity.GetUserId())
                    return RedirectToAction("Index", "Home");
                return View(checkingAccount);
            }

        }
 
      //  [Authorize(Roles = "Admin")]
        public ActionResult DetailsForAdmin(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var checkingAccount = db.CheckingAccounts.Find(id);
                return View("Details", checkingAccount);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            using (var db = new ApplicationDbContext())
            {
                return View(db.CheckingAccounts.ToList());
            }
        }

        public ActionResult Statement(int id)
        {
            var db = new ApplicationDbContext();
            //using (var db = new ApplicationDbContext())
            //{
            var checkingAccount = db.CheckingAccounts.Find(id);
            var userId = User.Identity.GetUserId();
            if (checkingAccount == null || checkingAccount.ApplicationUserId != User.Identity.GetUserId())
                return RedirectToAction("Index", "Home");
                var trans = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = 200,
                    CheckingAccountId = 15
                }
            };
            var transactions = checkingAccount.Transactions;
            return View(transactions);
            // }
        }
        // GET: CheckingAccount/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var checkingAccount = db.CheckingAccounts.Find(id);
                if (checkingAccount == null)
                    return RedirectToAction("List");
                return View(checkingAccount);
            }
        }

        // POST: CheckingAccount/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CheckingAccount checkingAccountModel)
        {
            using (var db = new ApplicationDbContext())
            {
                var checkingAccount = db.CheckingAccounts.Find(id);

                if (checkingAccount == null)
                    return RedirectToAction("List");
                checkingAccount.User = checkingAccountModel.User;
                
                if (ModelState.IsValid)
                {

                    checkingAccount.AccountNumber = checkingAccountModel.AccountNumber;
                    checkingAccount.FirstName = checkingAccountModel.FirstName;
                    checkingAccount.LastName = checkingAccountModel.LastName;
                    checkingAccount.Balance = checkingAccountModel.Balance;
                    checkingAccount.ApplicationUserId = checkingAccountModel.ApplicationUserId;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }

            return View("Edit", checkingAccountModel);

        }

        // GET: CheckingAccount/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var checkingAccount = db.CheckingAccounts.Find(id);

                if (checkingAccount == null)
                    return RedirectToAction("List");
                return View(checkingAccount);
            }
        }

        // POST: CheckingAccount/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (var db = new ApplicationDbContext())
            {
                var checkingAccount = db.CheckingAccounts.Find(id);

                if (checkingAccount == null)
                    return RedirectToAction("List");
                var user = db.Users.Find(checkingAccount.ApplicationUserId);
                db.Users.Remove(user);
                db.CheckingAccounts.Remove(checkingAccount);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
    }
}
