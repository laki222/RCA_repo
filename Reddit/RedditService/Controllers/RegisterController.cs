using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using RedditService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using RedditService.Repository;
using System.Diagnostics;

namespace RedditService.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

        private readonly UserDataRepository _userDataRepository;
        private CloudStorageAccount _storageAccount;
        private CloudTable userTable;
        private CloudBlobContainer blobContainer;

        public RegisterController()
        {
            _userDataRepository = new UserDataRepository();
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));



            var blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("userimages");
            blobContainer.CreateIfNotExists();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if user already exists
                    if (await _userDataRepository.UserExists(model.Email))
                    {
                        ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                        return View(model);
                    }

                    // Save image to Blob storage
                    var stopwatch = Stopwatch.StartNew();
                    var blobName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                    var blockBlob = blobContainer.GetBlockBlobReference(blobName);
                    await blockBlob.UploadFromStreamAsync(model.Image.InputStream);
                    model.ImageUrl = blockBlob.Uri.ToString();
                    stopwatch.Stop();
                    Trace.WriteLine($"Blob upload took {stopwatch.ElapsedMilliseconds} ms");

                    // Create a UserEntity from the model
                    var userEntity = new UserEntity(model.Email)
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        City = model.City,
                        Country = model.Country,
                        PhoneNumber = model.PhoneNumber,
                        Password = model.Password, // Note: Passwords should be hashed
                        ImageUrl = model.ImageUrl
                    };

                    // Add the user entity to Table storage using the repository
                    stopwatch.Restart();
                    await _userDataRepository.AddUser(userEntity);
                    stopwatch.Stop();
                    Trace.WriteLine($"Table insertion took {stopwatch.ElapsedMilliseconds} ms");

                    return View("Success", model);
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Exception occurred: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                    return View(model);
                }
            }
            return View(model);
        }
    }
}