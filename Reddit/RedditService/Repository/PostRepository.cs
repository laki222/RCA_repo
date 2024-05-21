using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RedditService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RedditService.Repository
{
    public class PostRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public PostRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            //CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("Posts"); _table.CreateIfNotExists();
        }

        public IQueryable<PostEntity> RetrieveAllPosts()
        {
            var results = from g in _table.CreateQuery<PostEntity>()
                          where g.PartitionKey == "Posts"
                          select g;
            return results;
        }

        public async Task AddPostAsync(PostEntity post)
        {
            var insertOperation = TableOperation.Insert(post);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task<PostEntity> GetPostAsync(int postId)
        {

            var retrieveOperation = TableOperation.Retrieve<PostEntity>("Posts", postId.ToString());
            var result = await _table.ExecuteAsync(retrieveOperation);
            return result.Result as PostEntity;
        }

        public async Task UpdatePostAsync(PostEntity post)
        {
            var updateOperation = TableOperation.Replace(post);
            await _table.ExecuteAsync(updateOperation);
        }
    }
}
