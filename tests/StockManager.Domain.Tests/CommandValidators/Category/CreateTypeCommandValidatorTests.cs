using System;
using StockManager.Domain.Commands.Category;
using StockManager.Domain.CommandValidators.Category;
using Xunit;

namespace StockManager.Domain.Tests.CommandValidators.Category
{
    public sealed class CreateTypeCommandValidatorTests
    {
        [Fact]
        public void GivenIdIsEmpty_ExpectFail()
        {
            var validator = new CreateTypeCommandValidator();
            var command = new CreateTypeCommand(Guid.Empty, "UNSC Chares");
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "CategoryId");
        }

        [Fact]
        public void GivenNameIsEmpty_ExpectFail()
        {
            var validator = new CreateTypeCommandValidator();
            var command = new CreateTypeCommand(Guid.NewGuid(), string.Empty);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "Name");
        }
    }
}