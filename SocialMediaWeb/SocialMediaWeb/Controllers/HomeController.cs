using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;

namespace SocialMediaWeb.Controllers
{
    public class HomeController : Controller
    {
        private UserRepository userRepository = new UserRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userRepository.InsertContactUs(model);
                    TempData["SuccessMessage"] = "Your message has been sent successfully.";
                    return RedirectToAction("Contact");
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError("", "An error occurred while sending your message.");
                }
            }

            return View(model);
        }
    }
}