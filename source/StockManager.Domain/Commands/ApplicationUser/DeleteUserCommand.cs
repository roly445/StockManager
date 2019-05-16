using System;
using MediatR;
using ResultMonad;
using StockManager.Core;

namespace StockManager.Domain.Commands.ApplicationUser
{
    public sealed class DeleteUserCommand : IRequest<ResultWithError<ErrorData>>
    {
        public DeleteUserCommand(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; }
    }
}