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
            var storageConnectionString = CloudConfigurationManager.GetSetting("DataConnectionStringLocal");
            var _storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("HealthCheck"); _table.CreateIfNotExists();
        }

     
        public async Task AddCheckAsync(HealthCheckEntity check)
        {
            var insertOperation = TableOperation.Insert(check);
            await _table.ExecuteAsync(insertOperation);
        }


        public async Task<List<HealthCheckEntity>> GetHealthChecksAsync(DateTime startTime)
        {
            string timeFilter = TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, startTime);

            TableQuery<HealthCheckEntity> query = new TableQuery<HealthCheckEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "HealthCheck"));
            List<HealthCheckEntity> entities = new List<HealthCheckEntity>();
            List<HealthCheckEntity> filterList = new List<HealthCheckEntity>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<HealthCheckEntity> resultSegment = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                entities.AddRange(resultSegment.Results);
            } while (token != null);


            foreach (var entity in entities) { 
                if(entity.Timestamp >  startTime)
                {
                    filterList.Add(entity);

                }
            
            }
            filterList.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

            return filterList;
        }






    }
}
