using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PNBank.Models;

namespace PNBank.Controllers
{
    public class HomeController : Controller
    {

        // GET /home/index        
        [Authorize]
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.First(c => c.ApplicationUserId == userId).Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            return View();
            }
            
        }

        // GET /home/about        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
                
        public ActionResult Contact()
        {
            ViewBag.TheMessage = "Having trouble? Send us a message.";            

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string message)
        {
            // TODO: send the message to HQ
            ViewBag.TheMessage = "Thanks, we got your message!";

            return PartialView("_ContactThanks");
        }
        
    }
}