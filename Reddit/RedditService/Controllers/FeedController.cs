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
        private readonly CloudBlobContainer blobContainer;
        private readonly UserDataRepository _userDataRepository;
        private readonly CommentRepository _commentRepository;


        public FeedController() {
            _userDataRepository = new UserDataRepository();
            _postRepository = new PostRepository();
            _commentRepository=new CommentRepository();
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("postimages");
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();


        }

        // GET: Feed
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);
            List<PostEntity> list = new List<PostEntity>();
            list = await _postRepository.RetrieveAllPosts();
            ViewBag.Posts = list;   


            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LikePost(string rowkey, bool isUpvote)
        {
            // Pretpostavimo da imate servis za upravljanje postovima
            UserEntity user = Session["UserProfile"] as UserEntity;

            var post = await _postRepository.GetPostAsync(rowkey);

            if (isUpvote)
            {
                post.Upvotes += 1;  
            }
            else if(!isUpvote )
            {  
                post.Downvotes += 1;
            }
            await _postRepository.UpdatePostAsync(post);


            // Možete se vratiti na istu stranicu ili neku drugu stranicu
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> Comments(string rowkey)
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);
            List<CommentEntity> list = new List<CommentEntity>();
            list = await _commentRepository.RetrieveAllComments();
            List<PostEntity> listpost = new List<PostEntity>();
            listpost = await _postRepository.RetrieveAllPosts();
            ViewBag.Posts = listpost;
            ViewBag.Comments = list;
            ViewBag.ButtonClick = "clicked";
           
           


            return View("Index");
        }
        [HttpPost]
        public async Task<ActionResult> AddComment(string postId, string content)
        {
            UserEntity test = Session["UserProfile"] as UserEntity;

            if (ModelState.IsValid)
            {
                try
                {
                    // "1058daa9-9ad8-4a09-86d6-ce2463dfb98e
                    var commentEntity = new CommentEntity()
                    {
                        PostId = postId,
                        Content = content,
                        AuthorEmail = test.RowKey
                    };

                    // Add the comment entity to the repository
                    await _commentRepository.AddCommentAsync(commentEntity); 
                    List<CommentEntity> list = new List<CommentEntity>();
                    list = await _commentRepository.RetrieveAllComments();
                    List<PostEntity> listpost = new List<PostEntity>();
                    listpost = await _postRepository.RetrieveAllPosts();
                    ViewBag.Posts = listpost;
                    ViewBag.Comments = list;
                    ViewBag.ButtonClick = "clicked";

                    // Redirect to a page displaying the post or any other page
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the comment.");
                }
            }
            // If model state is not valid or if an error occurred, return to the view
            return View();
        }

    }
}