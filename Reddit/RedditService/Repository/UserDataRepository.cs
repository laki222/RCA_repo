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
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));
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
            var userEntity = await _table.ExecuteAsync(retrieveOperation);
            /*
            return new User
            {
                Email = userEntity.RowKey, // Email is the RowKey
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Address = userEntity.Address,
                City = userEntity.City,
                Country = userEntity.Country,
                PhoneNumber = userEntity.PhoneNumber,
                //Image = userEntity.,
                ImageUrl = userEntity.ImageUrl
            };
            */
            return userEntity.Result as UserEntity;


        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            try
            {
                // Create a retrieve operation that takes a user entity.
                var retrieveOperation = TableOperation.Retrieve<UserEntity>("User", email);

                // Execute the retrieve operation
                var result = await _table.ExecuteAsync(retrieveOperation);

                // Get the user entity from the result
                var userEntity = result.Result as UserEntity;

                if (userEntity != null)
                {
                    // Map the UserEntity to User model
                    return userEntity;
                }
                else
                {
                    // User not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error retrieving user: " + ex.Message);
                throw;
            }
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
            var temp = GetUser(user.RowKey);

            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                temp.ImageUrl = user.ImageUrl;
            }

            temp.FirstName = user.FirstName;
            temp.LastName = user.LastName;
            temp.Address = user.Address;
            temp.City = user.City;
            temp.Country = user.Country;
            temp.PhoneNumber = user.PhoneNumber;

            TableOperation updateOperation = TableOperation.Replace(temp);
            _table.Execute(updateOperation);
        }
    }
}