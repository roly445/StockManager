using System;
using FluentValidation;
using StockManager.Core.Constants;
using StockManager.Domain.Commands.ApplicationUser;

namespace StockManager.Domain.CommandValidators.ApplicationUser
{
    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            this.RuleFor(x => x.Id).NotEqual(Guid.Empty).WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.UserName).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.NormalizedUserName).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.PasswordHash).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
        }
    }
}