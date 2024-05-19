using Microsoft.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RedditService.Models;
using RedditService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RedditService.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
    
        private readonly UserDataRepository _userRepository;
        private readonly CloudBlobContainer _blobContainer;

        public ProfileController()
        {
            _userRepository = new UserDataRepository();

            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
           // var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference("profileimages");
            _blobContainer.CreateIfNotExistsAsync().Wait();
        }

        /*
        public async Task<ActionResult> FetchUserByEmail(string email)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(email);

                if(user != null)
                {
                    Session["User"] = user;

                    return RedirectToAction("ViewProfile");
                }
                else
                {
                    ViewBag.Message = "UserNotFound";
                    return View("Error");
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }
        public ActionResult ViewProfile()
        {
            var user = Session["User"] as User;
            //this.Session["UserProfile"] = user;
            if (user != null)
            {
                return View(user);
            }
            else
            {
                // Redirect to another action if user is null
                return RedirectToAction("UserNotFound");
            }
        }
        */

        [HttpPost]
        public async Task<ActionResult> GetUserProfile(string email)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(email);

                if (user != null)
                {
                    //var user = userEntity.ToUser();
                    this.Session["User"] = user;
                    Console.WriteLine("User set in session: ");

                    return RedirectToAction("ViewProfile");
                }
                else
                {
                    ViewBag.Message = "UserNotFound";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ViewProfile(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            //this.Session["UserProfile"] = user;

            if (user != null)
            {
                this.Session["User"] = user;
                Console.WriteLine("User retrieved from session: ");
                return View(user);
            }
            else
            {
                Console.WriteLine("User not found in session.");
                return RedirectToAction("UserNotFound");
            }
        }
        public ActionResult UserNotFound()
        {
            // You can return a specific view or perform any other action here
            return View("UserNotFound");
        }
        /*
        [HttpGet]
        public async Task<ActionResult> ViewProfile(string email)
        {
            //var userEmail = HttpContext.Session.GetString("UserEmail");
            //var email = Session["UserProfile"].ToString();
            var user = await _userRepository.GetUserByEmailAsync(email);
            this.Session["UserProfile"] = user;
            if (user != null)
            {
                return View("ViewProfile", user);
            }

            //var user = await _userRepository.GetUserAsync(email);
            //if (user == null)
            //{
                //return RedirectToAction("Login", "Login");
            //}

            return View(user);
        }
        */
        [HttpGet]
        public async Task<ActionResult> EditProfile(string email)
        {
            var user = await _userRepository.GetUserAsync(email);
            this.Session["UserProfile"] = user;
            if (user != null)
            {
                return RedirectToAction("EditProfile", "Profile");
            }

            //var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return View("Login", user);
            }

            return View(user);
        }

        /*
        [HttpPost]
        public async Task<ActionResult> EditProfile(string email, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        // Save image to Blob storage
                        var blobName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var blockBlob = _blobContainer.GetBlockBlobReference(blobName);
                        await blockBlob.UploadFromStreamAsync(image.OpenReadStream());
                        model.ImageUrl = blockBlob.Uri.ToString();
                    }

                    await _userRepository.UpdateUserAsync(model);
                    HttpContext.Session.SetString("UserImageUrl", model.ImageUrl ?? string.Empty);
                    return RedirectToAction("ViewProfile");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                }
            }

            return View(model);
        }
        */
    }
}