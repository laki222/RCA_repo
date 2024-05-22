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
    public class CommentController : Controller
    {
        // GET: Comment

        private readonly CommentRepository _commentRepository;

        public CommentController()
        {
            _commentRepository = new CommentRepository();   
        }

        public ActionResult Comment()
        {
            List<CommentEntity> comments = new List<CommentEntity>();
            comments=ViewBag.Comments;
            var test = ViewBag.idpost;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(string postId, string content)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // "1058daa9-9ad8-4a09-86d6-ce2463dfb98e
                    var commentEntity = new CommentEntity()
                    {
                        PostId = postId,
                        Content = content,
                        AuthorEmail = (string)Session["UserProfile"]
                    };

                    // Add the comment entity to the repository
                    await _commentRepository.AddCommentAsync(commentEntity);

                    // Redirect to a page displaying the post or any other page
                    return RedirectToAction("Comment", "Comment");
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
