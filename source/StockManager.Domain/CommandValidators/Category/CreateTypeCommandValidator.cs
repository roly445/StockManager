using System;
using FluentValidation;
using StockManager.Core.Constants;
using StockManager.Domain.Commands.Category;

namespace StockManager.Domain.CommandValidators.Category
{
    public sealed class CreateTypeCommandValidator : AbstractValidator<CreateTypeCommand>
    {
        public CreateTypeCommandValidator()
        {
            this.RuleFor(x => x.CategoryId).NotEqual(Guid.Empty).WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.Name).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
        }
    }
}