using FluentValidation;
using StockManager.Domain.Commands.ApplicationUser;

namespace StockManager.Domain.CommandValidators.ApplicationUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
    }
}