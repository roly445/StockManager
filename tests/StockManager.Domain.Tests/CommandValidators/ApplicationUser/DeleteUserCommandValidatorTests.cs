using System;
using StockManager.Domain.Commands.ApplicationUser;
using StockManager.Domain.CommandValidators.ApplicationUser;
using Xunit;

namespace StockManager.Domain.Tests.CommandValidators.ApplicationUser
{
    public class DeleteUserCommandValidatorTests
    {
        [Fact]
        public void GivenUserIdIsEmpty_ExpectFail()
        {
            var validator = new DeleteUserCommandValidator();
            var command = new DeleteUserCommand(Guid.Empty);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "UserId");
        }
    }
}