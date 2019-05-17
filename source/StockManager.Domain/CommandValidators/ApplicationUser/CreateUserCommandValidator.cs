using FluentValidation;
using StockManager.Core.Constants;
using StockManager.Domain.Commands.ApplicationUser;

namespace StockManager.Domain.CommandValidators.ApplicationUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            this.RuleFor(x => x.UserName).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.NormalizedUserName).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.PasswordHash).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
        }
    }
}