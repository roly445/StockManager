using StockManager.Domain.Commands.Category;
using StockManager.Domain.CommandValidators.Category;
using Xunit;

namespace StockManager.Domain.Tests.CommandValidators.Category
{
    public sealed class CreateCategoryCommandValidatorTests
    {
        [Fact]
        public void GivenNameIsEmpty_ExpectFail()
        {
            var validator = new CreateCategoryCommandValidator();
            var command = new CreateCategoryCommand(string.Empty);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "Name");
        }
    }
}