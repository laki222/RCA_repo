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
    public class CommentRepository { 


        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public CommentRepository()
        {
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
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
    }
}