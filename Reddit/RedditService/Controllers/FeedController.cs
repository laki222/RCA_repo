using Microsoft.Azure;
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
    public class FeedController : Controller
    {
        private readonly PostRepository _postRepository;
        private CloudBlobContainer blobContainer;
        private readonly UserDataRepository _userDataRepository;



        public FeedController() {
            _userDataRepository = new UserDataRepository();
            _postRepository = new PostRepository();

            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("postimages");
            blobContainer.CreateIfNotExists();


        }

        // GET: Feed
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);
            List<PostEntity> list = new List<PostEntity>();
            list = _postRepository.RetrieveAllPosts().ToList();
            ViewBag.Posts = list;   


            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LikePost(int postId, bool isUpvote)
        {
            // Pretpostavimo da imate servis za upravljanje postovima
            

            if (isUpvote)
            {
                var post= await _postRepository.GetPostAsync(postId);
                post.Upvotes += 1;
            }
            else
            {
                var post=await _postRepository.GetPostAsync(postId);
                post.Downvotes -= 1;
            }

            // Možete se vratiti na istu stranicu ili neku drugu stranicu
            return RedirectToAction("Index");
        }

    }
}