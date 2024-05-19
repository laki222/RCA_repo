using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedditService.Models
{
    public static class UserMapper
    {
        public static User ToUser(this UserEntity entity)
        {
            return new User
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.RowKey, // Use RowKey as Email
                Password = entity.Password,
                ImageUrl = entity.ImageUrl
            };
        }
    }

}