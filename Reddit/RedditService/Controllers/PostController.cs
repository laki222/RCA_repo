using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RedditService.Models;
using RedditService.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RedditService.Controllers
{
    public class PostController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly UserDataRepository _userDataRepository;
        private CloudBlobContainer blobContainer;
        //private CloudStorageAccount storageAccount;

        public PostController()
        {
            _userDataRepository = new UserDataRepository();
            _postRepository = new PostRepository();

            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("postimages");
            blobContainer.CreateIfNotExists();
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(PostEntity model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save image to Blob storage

                    // Retrieve the currently logged-in user
                    UserEntity user = Session["UserProfile"] as UserEntity;
                    var currentUser = await _userDataRepository.GetUserByEmailAsync(user.RowKey); // You need to implement this method

                    // Create a PostEntity from the model
                    var postEntity = new PostEntity(currentUser)
                    {
                        Title = model.Title,
                        Content = model.Content,
                        ImageUrl = model.ImageUrl,
                        Upvotes = 0,
                        Downvotes = 0
                    };

                    // Add the post entity to Table storage using the repository
                    await _postRepository.AddPostAsync(postEntity);

                    return View("Success", model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                    return View(model);
                }
            }
            return View(model);
        }

    }
}