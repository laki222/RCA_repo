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

        public async Task<List<PostEntity>> RetrieveAllPosts()
        {
            TableQuery<PostEntity> query = new TableQuery<PostEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Posts"));
            List<PostEntity> entities = new List<PostEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<PostEntity> resultSegment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                entities.AddRange(resultSegment.Results);
            } while (token != null);

            return  entities;
        }

        public async Task AddPostAsync(PostEntity post)
        {
            var insertOperation = TableOperation.Insert(post);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task<PostEntity> GetPostAsync(string rowkey)
        {

            var retrieveOperation = TableOperation.Retrieve<PostEntity>("Posts", rowkey);
            var result = await _table.ExecuteAsync(retrieveOperation);
            return result.Result as PostEntity;
        }
        public async Task<PostEntity> GetPost(string rowkey)
        {
            //return RetrieveAllPosts().Wait()(p => p.RowKey == rowkey).FirstOrDefault();

            List<PostEntity> lista = await RetrieveAllPosts();
            return lista.Find(p => p.RowKey == rowkey);
        }

        public async Task UpdatePostAsync(PostEntity post)
        {
            await Task.Delay(100);

            TableOperation updateOperation = TableOperation.Replace(post);
            _table.Execute(updateOperation);
        }
    }
}
