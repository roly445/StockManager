using System;

namespace StockManager.Queries.TransferObjects
{
    public sealed class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string PasswordHash { get; set; }
    }
}