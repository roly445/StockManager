using System;
using MediatR;
using ResultMonad;
using StockManager.Core;

namespace StockManager.Domain.Commands.ApplicationUser
{
    public sealed class UpdateUserCommand : IRequest<ResultWithError<ErrorData>>
    {
        public UpdateUserCommand(Guid id, string userName, string normalizedUserName, string passwordHash)
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