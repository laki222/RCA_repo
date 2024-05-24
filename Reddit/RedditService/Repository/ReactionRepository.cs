using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using System.Web.UI.WebControls;
using RedditService.Models;
using System.Threading.Tasks;

namespace RedditService.Repository
{
   
    public class ReactionRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public ReactionRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            //CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("Reactions"); _table.CreateIfNotExists();
        }

        public async Task<List<ReactionEntity>> RetrieveAllReactions()
        {
            TableQuery<ReactionEntity> query = new TableQuery<ReactionEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Reactions"));
            List<ReactionEntity> entities = new List<ReactionEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<ReactionEntity> resultSegment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                entities.AddRange(resultSegment.Results);
            } while (token != null);

            return entities;
        }


        public async Task AddReactionAsync(ReactionEntity reaction)
        {
            var insertOperation = TableOperation.Insert(reaction);
            await _table.ExecuteAsync(insertOperation);
        }

        public async Task UpdateReactionAsync(ReactionEntity reaction)
        {
            await Task.Delay(100);

            TableOperation updateOperation = TableOperation.Replace(reaction);
            _table.Execute(updateOperation);
        }

        public async Task<List<ReactionEntity>> RetrieveUpVotesByEmail(string email)
        {
            TableQuery<ReactionEntity> query = new TableQuery<ReactionEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Reactions"));
            List<ReactionEntity> entities = new List<ReactionEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<ReactionEntity> resultSegment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                entities.AddRange(resultSegment.Results);
            } while (token != null);

            return entities.FindAll(p=>p.SubscribedUser==email && p.Reaction=="UPVOTE");

            
        }


    }
}