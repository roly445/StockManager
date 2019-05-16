using System;

namespace StockManager.Web.Infrastructure.Stores
{
    public class AppUser
    {
        public AppUser(Guid id, string normalizedUserName, string userName, string passwordHash)
        {
            this.Id = id;
            this.NormalizedUserName = normalizedUserName;
            this.UserName = userName;
            this.PasswordHash = passwordHash;
        }

        public Guid Id { get; private set; }

        public string NormalizedUserName { get; private set; }

        public string UserName { get; private set; }

        public string PasswordHash { get; private set; }

        public void UpdateId(Guid newId)
        {
            this.Id = newId;
        }

        public void UpdateUserName(string newUserName)
        {
            this.UserName = newUserName;
        }

        public void UpdateNormalizedUserName(string newNormalizedUserName)
        {
            this.NormalizedUserName = newNormalizedUserName;
        }

        public void UpdatePasswordHash(string newPasswordHash)
        {
            this.PasswordHash = newPasswordHash;
        }
    }
}