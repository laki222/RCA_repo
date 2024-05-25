using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringService.Model
{
    public class HealthCheckEntity : TableEntity
    {
        
        public string Name { get; set; }
        public string Status { get; set; }

        public HealthCheckEntity() { }

        public HealthCheckEntity(string status, DateTime timestamp, string name)
        {
            PartitionKey = "HealthCheck";
            RowKey = timestamp.ToString("o");
            Status = status;
            Name = name;
        }

    }
}