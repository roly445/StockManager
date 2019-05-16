using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Domain.CommandResults.ApplicationUser;

namespace StockManager.Domain.Commands.ApplicationUser
{
    public class CreateUserCommand : IRequest<Result<CreateUserCommandResult, ErrorData>>
    {
        public CreateUserCommand(string userName, string normalizedUserName, string passwordHash)
        {
            this.UserName = userName;
            this.NormalizedUserName = normalizedUserName;
            this.PasswordHash = passwordHash;
        }

        public string UserName { get; }

        public string NormalizedUserName { get; }

        public string PasswordHash { get; }
    }
}