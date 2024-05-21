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
            _blobContainer = blobClient.GetContainerReference("userimages");
            _blobContainer.CreateIfNotExistsAsync().Wait();
            _blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();
        }

      
        [HttpGet]
        public async Task<ActionResult> ViewProfile()
        {
            await Task.Delay(100);
            var user = this.Session["UserProfile"] as UserEntity;
            ViewBag.IsUserLoggedIn = "true";

            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(user.ImageUrl), _blobContainer.ServiceClient.Credentials);
                    if (await blockBlob.ExistsAsync())
                    {
                        user.ImageUrl = blockBlob.Uri.ToString();
                    }
                }
                Console.WriteLine("User retrieved from session: ");
                return View(user);
            }
            else
            {
                Console.WriteLine("User not found in session.");
                return RedirectToAction("UserNotFound");
            }
        }
       
     
        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            await Task.Delay(100);
            ViewBag.IsUserLoggedIn = "true";
            var user = this.Session["UserProfile"];
            if (user != null)
            {
                return View("EditProfile",user);
            }

            //var user = await _userRepository.GetUserByEmailAsync(userEmail);
            else 
            {
                return View("Login", user);
            }

           
        }

        [HttpPost]
        public async Task<ActionResult> SaveProfile(UserEntity user)
        {
            await Task.Delay(100);
            ViewBag.IsUserLoggedIn = "true";
            var sesija = Session["UserProfile"] as UserEntity;

            user.RowKey = sesija.RowKey;
            user.Password= sesija.Password;

            if (user != null) {

                _userRepository.UpdateUser(user);
                Session["UserProfile"]=user;
                return View("Success", user);
            }
            else
            {
                Console.WriteLine("User not found in session.");
                return RedirectToAction("UserNotFound");
            }
        }



    }
}