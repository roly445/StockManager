using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Moq;
using StockManager.Core.Constants;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;
using StockManager.Domain.CommandHandlers.ApplicationUser;
using StockManager.Domain.Commands.ApplicationUser;
using StockManager.Queries.Contracts;
using StockManager.Queries.Models;
using Xunit;

namespace StockManager.Domain.Tests.CommandHandlers.ApplicationUser
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_GivenUserExists_ShouldReturnUserExistsError()
        {
            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            var identityQueries = new Mock<IIdentityQueries>();
            identityQueries.Setup(x => x.CheckForPresenceOfUserByNormalizedUserName(It.IsAny<string>())).ReturnsAsync(() =>
                new StatusCheckModel(true));
            var createUserCommandHandler =
                new CreateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new CreateUserCommand("Emile-A239", "emile-a239", "some-hash");
            var result = await createUserCommandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.UserAlreadyExists, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSaveFails_ShouldReturnSavingChangesError()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => false);
            var applicationUserRepository = new Mock<IApplicationUserRepository>();

            applicationUserRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var identityQueries = new Mock<IIdentityQueries>();
            identityQueries.Setup(x => x.CheckForPresenceOfUserByNormalizedUserName(It.IsAny<string>())).ReturnsAsync(() =>
                new StatusCheckModel(false));
            var createUserCommandHandler =
                new CreateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new CreateUserCommand("Emile-A239", "emile-a239", "some-hash");
            var result = await createUserCommandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.SavingChanges, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSuccess_ShouldReturnSuccessfulResult()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => true);
            var applicationUserRepository = new Mock<IApplicationUserRepository>();

            applicationUserRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var identityQueries = new Mock<IIdentityQueries>();
            identityQueries.Setup(x => x.CheckForPresenceOfUserByNormalizedUserName(It.IsAny<string>())).ReturnsAsync(() =>
                new StatusCheckModel(false));
            var createUserCommandHandler =
                new CreateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new CreateUserCommand("Emile-A239", "emile-a239", "some-hash");
            var result = await createUserCommandHandler.Handle(command, default);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value.UserId);
        }
    }
}