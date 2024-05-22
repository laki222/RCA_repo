using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace RedditService.Models
{
    public class CommentEntity : TableEntity
    {
        public CommentEntity(PostEntity post)
        {
            this.PartitionKey = "Comments";
            this.RowKey = Guid.NewGuid().ToString();
            this.PostId = post.RowKey;

        }
        public CommentEntity()
        {
            this.PartitionKey = "Comments";
            this.RowKey = Guid.NewGuid().ToString();

        }

        public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public string AuthorEmail { get; set; }
        public bool IsDeleted { get; set; }
    }
}