using FluentValidation;
using StockManager.Domain.Commands.ApplicationUser;

namespace StockManager.Domain.CommandValidators.ApplicationUser
{
    public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
    }
}