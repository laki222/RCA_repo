using HealthMonitoringService.Model;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringService.Repository
{
    public class HealthCheckRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public HealthCheckRepository()
        {
            var storageConnectionString = CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");
            var _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("HealthCheck"); _table.CreateIfNotExists();
        }

     
        public async Task AddCheckAsync(HealthCheckEntity check)
        {
            var insertOperation = TableOperation.Insert(check);
            await _table.ExecuteAsync(insertOperation);
        }

      

    }
}
