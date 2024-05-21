using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedditService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var user = Session["UserProfile"];

            if (user != null)
            {
                ViewBag.IsUserLoggedIn= "true";
                return View();
            }
            else
            {
                return View();
            }
           
        }

        public ActionResult Contact()
        {
            var user = Session["UserProfile"];

            if (user != null)
            {
                ViewBag.IsUserLoggedIn = "true";
                return View();
            }
            else
            {
                return View();
            }
            
        }


        public ActionResult Register()
        {
            return RedirectToAction("Register", "Register");
        }
    }
}