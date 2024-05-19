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
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("Comments"); _table.CreateIfNotExists();
        }

        public async Task AddCommentAsync(CommentEntity comment)
        {
            var insertOperation = TableOperation.Insert(comment);
            await _table.ExecuteAsync(insertOperation);
        }
    }
}