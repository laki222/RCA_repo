using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using RedditService.Models;
using RedditService.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace RedditService.Controllers
{
    public class FeedController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly CloudBlobContainer blobContainer;
        private readonly UserDataRepository _userDataRepository;
        private readonly CommentRepository _commentRepository;
        private readonly ReactionRepository _reactionRepository;
        private CloudQueue queue;

        public FeedController()
        {
            _userDataRepository = new UserDataRepository();
            _postRepository = new PostRepository();
            _commentRepository = new CommentRepository();
            _reactionRepository = new ReactionRepository();
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionStringLocal");
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);


            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            queue = queueClient.GetQueueReference("notifications");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExistsAsync().Wait();

            var blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("postimages");
            blobContainer.CreateIfNotExists();
            //blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();


        }

        // GET: Feed
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pageSize = 3)
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);

            // Retrieve all posts
            List<PostEntity> list = await _postRepository.RetrieveAllPosts();
            List<PostEntity> filteredPosts = list.Where(post => !post.IsDeleted).ToList();
            // Get the current user's email
            UserEntity currentUser = Session["UserProfile"] as UserEntity;
            ViewBag.EmailOwnerPost = currentUser?.RowKey;

            // Calculate total number of pages
            int totalPosts = filteredPosts.Count;
            int totalPages = (int)Math.Ceiling((decimal)totalPosts / pageSize);




            // Get the posts for the current page
            List<PostEntity> paginatedPosts = filteredPosts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Pass paginated posts, total pages, and current page to the view
            ViewBag.PaginatedPosts = paginatedPosts;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LikePost(string rowkey, bool isUpvote)
        {
            // Pretpostavimo da imate servis za upravljanje postovima
            UserEntity user = Session["UserProfile"] as UserEntity;

            var post = await _postRepository.GetPostAsync(rowkey);

            List<ReactionEntity> reactions = await _reactionRepository.RetrieveAllReactions();

            // Pronađite postojeću reakciju korisnika na post
            var existingReaction = reactions.FirstOrDefault(r => r.SubscribedUser == user.RowKey && r.PostId == post.RowKey);

            if (existingReaction != null)
            {
                if (existingReaction.Reaction == "UPVOTE")
                {
                    ViewBag.react = "up";
                }
                else
                {
                    ViewBag.react = "down";
                }

                // Postoji reakcija, potrebno je ažurirati
                if (isUpvote && existingReaction.Reaction != "UPVOTE")
                {
                    // Ako je korisnik kliknuo na upvote, a prethodna reakcija je bila downvote
                    post.Upvotes += 1;
                    post.Downvotes -= 1;
                    existingReaction.Reaction = "UPVOTE";

                    await _reactionRepository.UpdateReactionAsync(existingReaction);
                }
                else if (!isUpvote && existingReaction.Reaction != "DOWNVOTE")
                {
                    // Ako je korisnik kliknuo na downvote, a prethodna reakcija je bila upvote
                    post.Downvotes += 1;
                    post.Upvotes -= 1;
                    existingReaction.Reaction = "DOWNVOTE";
                    await _reactionRepository.UpdateReactionAsync(existingReaction);
                }
            }
            else
            {
                // Nema postojeće reakcije, dodajte novu
                if (isUpvote)
                {
                    post.Upvotes += 1;
                    ViewBag.react = "up";
                    await _reactionRepository.AddReactionAsync(new ReactionEntity(post, "UPVOTE", user.RowKey));
                }
                else
                {
                    post.Downvotes += 1;
                    ViewBag.react = "down";
                    await _reactionRepository.AddReactionAsync(new ReactionEntity(post, "DOWNVOTE", user.RowKey));
                }
            }

            await _postRepository.UpdatePostAsync(post);

            // Možete se vratiti na istu stranicu ili neku drugu stranicu
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<ActionResult> Comments(string rowkey, int page = 1, int pageSize = 3)
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);

            List<CommentEntity> list = await _commentRepository.RetrieveAllComments();
            List<PostEntity> listpost = await _postRepository.RetrieveAllPosts();
            List<PostEntity> filteredPosts = listpost.Where(post => !post.IsDeleted).ToList();
            ViewBag.Posts = listpost;
            ViewBag.Comments = list;
            UserEntity test = Session["UserProfile"] as UserEntity;

            ViewBag.EmailOwnerPost = test.RowKey;
            var previousRowKey = TempData["ClickedPostRowKey"] as string;

            if (previousRowKey == rowkey)
            {
                ViewBag.ButtonClick = null;
                ViewBag.ClickedPostRowKey = null;
            }
            else
            {
                ViewBag.ButtonClick = "clicked";
                ViewBag.ClickedPostRowKey = rowkey;
            }

            TempData["ClickedPostRowKey"] = ViewBag.ClickedPostRowKey;
            int totalPosts = filteredPosts.Count;
            int totalPages = (int)Math.Ceiling((decimal)totalPosts / pageSize);

            // Get the posts for the current page
            List<PostEntity> paginatedPosts = filteredPosts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Pass paginated posts, total pages, and current page to the view
            ViewBag.PaginatedPosts = paginatedPosts;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
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

                    string messageText = Newtonsoft.Json.JsonConvert.SerializeObject(commentEntity);

                    // Create a new queue message
                    CloudQueueMessage message = new CloudQueueMessage(messageText);

                    // Add the message to the queue
                    await queue.AddMessageAsync(message);


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


        [HttpPost]
        public async Task<ActionResult> DeleteComment(string commentId)
        {

            var comment = await _commentRepository.GetComment(commentId);
            await _commentRepository.DeleteCommentAsync(comment);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Search(string titleFilter, int page = 1, int pageSize = 3)
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);

            List<CommentEntity> list = await _commentRepository.RetrieveAllComments();
            List<PostEntity> listpost = await _postRepository.RetrieveAllPosts();


            ViewBag.Comments = list;


            if (!string.IsNullOrEmpty(titleFilter))
            {
                // Filter the posts based on the title containing the input text
                listpost = listpost.Where(p => p.IsDeleted == false && p.Title.Contains(titleFilter)).ToList();
            }


            int totalPosts = listpost.Count;
            int totalPages = (int)Math.Ceiling((decimal)totalPosts / pageSize);

            // Get the posts for the current page
            List<PostEntity> paginatedPosts = listpost.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Pass paginated posts, total pages, and current page to the view
            ViewBag.PaginatedPosts = paginatedPosts;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;



            ViewBag.Posts = listpost;
            ViewBag.Comments = await _commentRepository.RetrieveAllComments();

            return View("Index");


        }
        public async Task<ActionResult> Sort(string sortBy, string sortOrder, int page = 1, int pageSize = 3)
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);

            List<PostEntity> posts = await _postRepository.RetrieveAllPosts();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
            {
                posts = SortPosts(posts, sortBy, sortOrder);

                // Toggle the sorting order for the next request
                TempData["SortOrder"] = sortOrder == "ascending" ? "descending" : "ascending";
            }

            ViewBag.Posts = posts;

            return View("Index");
        }

        private List<PostEntity> SortPosts(List<PostEntity> posts, string sortBy, string sortOrder)
        {
            switch (sortBy)
            {
                case "title":
                    posts = sortOrder == "ascending" ? posts.OrderBy(p => p.Title).ToList() : posts.OrderByDescending(p => p.Title).ToList();
                    break;
                case "upvotes":
                    posts = sortOrder == "ascending" ? posts.OrderBy(p => p.Upvotes).ToList() : posts.OrderByDescending(p => p.Upvotes).ToList();
                    break;
                    // Add more cases for other sorting criteria if needed
            }

            return posts;
        }

        public ActionResult ShowCreatePost()
        {
            return PartialView("_CreatePostPartial");
        }

        public async Task<ActionResult> MyPosts()
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);
            UserEntity user = Session["UserProfile"] as UserEntity;
            List<PostEntity> posts = await _postRepository.RetrieveAllPosts();

            List<PostEntity> UserPostsFilter = new List<PostEntity>();

            foreach (PostEntity post in posts)
            {
                if (post.IsDeleted == false && post.AuthorEmail == user.RowKey)
                {
                    UserPostsFilter.Add(post);
                }
            }

            List<ReactionEntity> reactions = await _reactionRepository.RetrieveUpVotesByEmail(user.RowKey);

            List<PostEntity> UserPostsUpvoted = new List<PostEntity>();



            foreach (ReactionEntity react in reactions)
            {

                UserPostsUpvoted.Add(await _postRepository.GetPostAsync(react.PostId));
            }

            List<PostEntity> filteredPosts = UserPostsUpvoted.Where(post => !post.IsDeleted).ToList();


            ViewBag.UpVoted = filteredPosts;


            ViewBag.UserPosts = UserPostsFilter;


            return View("MyPosts");
        }

        public async Task<ActionResult> DeletePost(string postId)
        {
            ViewBag.IsUserLoggedIn = "true";
            var post = await _postRepository.GetPost(postId);
            await _postRepository.DeletePostAsync(post);
            UserEntity user = Session["UserProfile"] as UserEntity;
            List<PostEntity> posts = await _postRepository.RetrieveAllPosts();

            List<PostEntity> UserPostsFilter = new List<PostEntity>();

            foreach (PostEntity p in posts)
            {
                if (p.IsDeleted == false && p.AuthorEmail == user.RowKey)
                {
                    UserPostsFilter.Add(p);
                }
            }

            List<ReactionEntity> reactions = await _reactionRepository.RetrieveUpVotesByEmail(user.RowKey);

            List<PostEntity> UserPostsUpvoted = new List<PostEntity>();



            foreach (ReactionEntity react in reactions)
            {

                UserPostsUpvoted.Add(await _postRepository.GetPostAsync(react.PostId));
            }

            List<PostEntity> filteredPosts = UserPostsUpvoted.Where(p => !p.IsDeleted).ToList();


            ViewBag.UpVoted = filteredPosts;




            ViewBag.UserPosts = UserPostsFilter;
            return View("MyPosts");


        }

        public async Task<ActionResult> UpvotedPosts()
        {
            ViewBag.IsUserLoggedIn = "true";
            await Task.Delay(100);
            UserEntity user = Session["UserProfile"] as UserEntity;
            List<ReactionEntity> reactions = await _reactionRepository.RetrieveUpVotesByEmail(user.RowKey);

            List<PostEntity> UserPostsUpvoted = new List<PostEntity>();

            foreach (ReactionEntity react in reactions)
            {

                UserPostsUpvoted.Add(await _postRepository.GetPostAsync(react.PostId));
            }

            ViewBag.UpVoted = UserPostsUpvoted;

            return View("MyPosts");
        }


    }
}