using System;
using StockManager.Domain.Commands.ApplicationUser;
using StockManager.Domain.CommandValidators.ApplicationUser;
using Xunit;

namespace StockManager.Domain.Tests.CommandValidators.ApplicationUser
{
    public class UpdateUserCommandValidatorTests
    {
        [Fact]
        public void GivenNormalizedUserNameIsEmpty_ExpectFail()
        {
            var validator = new UpdateUserCommandValidator();
            var command = new UpdateUserCommand(Guid.NewGuid(), "Emile-A239", string.Empty, "some-hash");
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "NormalizedUserName");
        }

        [Fact]
        public void GivenPasswordHashIsEmpty_ExpectFail()
        {
            var validator = new UpdateUserCommandValidator();
            var command = new UpdateUserCommand(Guid.NewGuid(), "Emile-A239", "emile-a239", string.Empty);
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "PasswordHash");
        }

        [Fact]
        public void GivenUserIdIsEmpty_ExpectFail()
        {
            var validator = new UpdateUserCommandValidator();
            var command = new UpdateUserCommand(Guid.Empty, "Emile-A239", "emile-a239", "some-hash");
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "Id");
        }

        [Fact]
        public void GivenUserNameIsEmpty_ExpectFail()
        {
            var validator = new UpdateUserCommandValidator();
            var command = new UpdateUserCommand(Guid.NewGuid(), string.Empty, "emile-a239", "some-hash");
            var result = validator.Validate(command);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, o => o.PropertyName == "UserName");
        }
    }
}