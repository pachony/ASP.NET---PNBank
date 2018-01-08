using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomatedTellerMachine;
using PNBank.Controllers;

namespace AutomatedTellerMachine.Tests.ControllersTest
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void ContactFormSaysThanks()
        {
            var homeController = new HomeController();
            var result = homeController.Contact("I love your bank") as PartialViewResult;
            Assert.IsNotNull(result.ViewBag.TheMessage);
        }

        [TestMethod]
        public void About()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.About() as ViewResult;
            
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.Contact() as ViewResult;
            
            Assert.IsNotNull(result);
        }
    }
}
