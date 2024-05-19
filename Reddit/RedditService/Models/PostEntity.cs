using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedditService.Models
{
    public class PostEntity : TableEntity
    {
        public PostEntity(User user)
        {
            this.PartitionKey = "Posts";
            this.RowKey = Guid.NewGuid().ToString();
            this.AuthorEmail = user.Email; // Set AuthorEmail to user's email
        }

        public PostEntity() { }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorEmail { get; set; }
        public string ImageUrl { get; set; }
        public List<string> CommentId { get; set; } 
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}