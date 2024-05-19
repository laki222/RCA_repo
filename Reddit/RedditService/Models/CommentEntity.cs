using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedditService.Models
{
    public class CommentEntity : TableEntity
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public string AuthorEmail { get; set; }
    }
}