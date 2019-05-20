using System;
using FluentValidation;
using StockManager.Core.Constants;
using StockManager.Domain.Commands.Stock;

namespace StockManager.Domain.CommandValidators.Stock
{
    public class AddBatchCommandValidator : AbstractValidator<AddBatchCommand>
    {
        public AddBatchCommandValidator()
        {
            this.RuleFor(x => x.TypeId).NotEqual(Guid.Empty).WithErrorCode(ValidationCodes.FieldIsRequired);
            this.RuleFor(x => x.Quantity).GreaterThan(0).WithErrorCode(ValidationCodes.FieldIsRequired);
        }
    }
}