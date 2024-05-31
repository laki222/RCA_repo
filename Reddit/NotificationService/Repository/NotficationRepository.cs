using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using RedditService.Models;
using NotificationService.Models;

namespace NotificationService.Repository
{
    public class NotficationRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public NotficationRepository()
        {

            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionStringLocal");
            var _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("NotificationLog"); _table.CreateIfNotExists();

        }

        public async Task AddLogAsync(NotificationLogEntity log)
        {
            var insertOperation = TableOperation.Insert(log);
            await _table.ExecuteAsync(insertOperation);
        }
    }
}
