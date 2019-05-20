using FluentValidation;
using StockManager.Core.Constants;
using StockManager.Domain.Commands.Category;

namespace StockManager.Domain.CommandValidators.Category
{
    public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty().WithErrorCode(ValidationCodes.FieldIsRequired);
        }
    }
}