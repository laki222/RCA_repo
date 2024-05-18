using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using RedditService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.Azure;
using System.Threading.Tasks;

namespace RedditService.Repository
{
    public class UserDataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public UserDataRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("UserTable"); _table.CreateIfNotExists();
        }
        public IQueryable<UserEntity> RetrieveAllUsers()
        {
            var results = from g in _table.CreateQuery<UserEntity>()
                          where g.PartitionKey == "User"
                          select g;
            return results;
        }
        public async Task AddUser(UserEntity userEntity)
        {
            var insertOperation = TableOperation.Insert(userEntity);
            await _table.ExecuteAsync(insertOperation);
        }
        public async Task<UserEntity> GetUserAsync(string email)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>("User", email);
            var result = await _table.ExecuteAsync(retrieveOperation);
            return result.Result as UserEntity;
        }
        public async Task<bool> UserExists(string email)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>("User", email);
            var result = await _table.ExecuteAsync(retrieveOperation);
            return result.Result != null;
        }

        public void RemoveUser(string id)
        {
            UserEntity user = RetrieveAllUsers().Where(s => s.RowKey == id).FirstOrDefault();

            if (user != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(user);
                _table.Execute(deleteOperation);
            }
        }

        public UserEntity GetUser(string email)
        {
            return RetrieveAllUsers().Where(p => p.RowKey == email).FirstOrDefault();
        }

        public void UpdateUser(UserEntity user)
        {
            TableOperation updateOperation = TableOperation.Replace(user);
            _table.Execute(updateOperation);
        }
    }
}