using System;
using System.Collections.Generic;
using System.Text;

namespace StockManager.Queries.Models
{
    public sealed class UserModel
    {
        public UserModel(Guid id, string userName, string normalizedUserName, string passwordHash)
        {
            this.Id = id;
            this.UserName = userName;
            this.NormalizedUserName = normalizedUserName;
            this.PasswordHash = passwordHash;
        }

        public Guid Id { get; }

        public string UserName { get; }

        public string NormalizedUserName { get; }

        public string PasswordHash { get; }
    }
}