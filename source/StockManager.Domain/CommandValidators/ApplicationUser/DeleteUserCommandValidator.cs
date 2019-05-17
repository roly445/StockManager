using System;
using FluentValidation;
using StockManager.Core.Constants;
using StockManager.Domain.Commands.ApplicationUser;

namespace StockManager.Domain.CommandValidators.ApplicationUser
{
    public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            this.RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithErrorCode(ValidationCodes.FieldIsRequired);
        }
    }
}