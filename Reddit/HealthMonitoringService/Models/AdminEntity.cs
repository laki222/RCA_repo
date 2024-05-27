using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringService.Models
{
    public class AdminEntity:TableEntity
    {
        
        public string Password { get; set; }

        public AdminEntity(string email,string password) {
        RowKey = email;
        PartitionKey = "Admins";
        Password= password;

        }
        public AdminEntity() { }

    }
}
