using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedditService.Models
{

    public enum REACTION { UPVOTE,DOWNVOTE};

    public class ReactionEntity: TableEntity
    {
        public ReactionEntity(PostEntity post,string r,string u)
        {
            this.PartitionKey = "Reactions";
            this.RowKey = Guid.NewGuid().ToString();
            this.PostId = post.RowKey;
            Reaction = r;
            SubscribedUser = u;
        }
        public ReactionEntity()
        {
            this.PartitionKey = "Comments";
            this.RowKey = Guid.NewGuid().ToString();

        }

        public string Id { get; set; }
        public string PostId { get; set; }
        public string SubscribedUser { get; set; }
        public string Reaction { get; set; }

    }
}

//ime / koje