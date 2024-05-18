using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace RedditService.Models
{
    public class UserEntity: TableEntity
    {
        public UserEntity(string email)
        {
            this.PartitionKey = "User";
            this.RowKey = email;
        }

        public UserEntity() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }

    }
}