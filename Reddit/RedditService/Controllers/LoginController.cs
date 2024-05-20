using RedditService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RedditService.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDataRepository _userDataRepository;
        // GET: Login

        public LoginController()
        {
            _userDataRepository = new UserDataRepository();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            var user = await _userDataRepository.GetUserAsync(email);

            if (user != null && user.Password == password) // Ensure passwords are hashed and compared securely in real applications
            {
                Session["UserProfile"] = user;
                return View("Success", user);
                //return RedirectToAction("ViewProfile", "Profile");

            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(user);
        }
    }
        
}
