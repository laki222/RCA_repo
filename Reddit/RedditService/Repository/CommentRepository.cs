using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RedditService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace RedditService.Repository
{
    public class CommentRepository
    {


        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public CommentRepository()
        {
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionStringLocal");
            var _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("Comments"); _table.CreateIfNotExists();
        }

        public async Task<List<CommentEntity>> RetrieveAllComments()
        {
            TableQuery<CommentEntity> query = new TableQuery<CommentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Comments"));
            List<CommentEntity> entities = new List<CommentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<CommentEntity> resultSegment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                entities.AddRange(resultSegment.Results);
            } while (token != null);

            return entities;
        }


        public async Task AddCommentAsync(CommentEntity comment)
        {
            var insertOperation = TableOperation.Insert(comment);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task DeleteCommentAsync(CommentEntity comment)
        {
            await Task.Delay(100);

            comment.IsDeleted = true;

            TableOperation updateOperation = TableOperation.Replace(comment);
            _table.Execute(updateOperation);
        }

        public async Task<CommentEntity> GetComment(string commId)
        {
            //return RetrieveAllPosts().Wait()(p => p.RowKey == rowkey).FirstOrDefault();

            List<CommentEntity> lista = await RetrieveAllComments();
            return lista.Find(c => c.RowKey == commId);
        }
    }
}