using System;

namespace StockManager.Domain.CommandResults.ApplicationUser
{
    public class CreateUserCommandResult
    {
        public CreateUserCommandResult(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; }
    }
}