using System;
using StockManager.Core.Domain;

namespace StockManager.Domain.AggregatesModel.ApplicationUserAggregate
{
    public class ApplicationUser : Entity, IApplicationUser
    {
        public ApplicationUser(Guid id, string userName, string normalizedUserName, string passwordHash)
        {
            this.Id = id;
            this.UserName = userName;
            this.NormalizedUserName = normalizedUserName;
            this.PasswordHash = passwordHash;
        }

        public string UserName { get; private set; }

        public string NormalizedUserName { get; private set; }

        public string PasswordHash { get; private set; }

        public void UpdateDetails(string userName, string normalizedUserName, string passwordHash)
        {
            this.UserName = userName;
            this.NormalizedUserName = normalizedUserName;
            this.PasswordHash = passwordHash;
        }
    }
}