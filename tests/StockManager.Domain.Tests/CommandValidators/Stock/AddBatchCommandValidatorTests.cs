using System;
using StockManager.Domain.Commands.Stock;
using StockManager.Domain.CommandValidators.Stock;
using Xunit;

namespace StockManager.Domain.Tests.CommandValidators.Stock
{
    public sealed class AddBatchCommandValidatorTests
    {
        [Fact]
        public void GivenIdIsEmpty_ExpectFail()
        {
            var validator = new AddBatchCommandValidator();
            var command = new AddBatchCommand(Guid.Empty, 1);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "TypeId");
        }

        [Fact]
        public void GivenQuantityIsLessThanZero_ExpectFail()
        {
            var validator = new AddBatchCommandValidator();
            var command = new AddBatchCommand(Guid.NewGuid(), -3);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "Quantity");
        }

        [Fact]
        public void GivenQuantityIsZero_ExpectFail()
        {
            var validator = new AddBatchCommandValidator();
            var command = new AddBatchCommand(Guid.NewGuid(), 0);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "Quantity");
        }
    }
}