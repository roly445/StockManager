using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
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
    public class UpdateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_GivenDifferentUserExistsWithSameUserName_ShouldReturnUserExistsError()
        {
            var userId = Guid.NewGuid();
            var applicationUser = new Mock<IApplicationUser>();
            applicationUser.Setup(x => x.Id).Returns(userId);

            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(applicationUser.Object));

            var identityQueries = new Mock<IIdentityQueries>();
            identityQueries.Setup(x => x.GetUserByNormalizedUserName(It.IsAny<string>())).ReturnsAsync(() =>
                Maybe.From(new UserModel(Guid.NewGuid(), "Emile-A239", "emile-a239", "some-hash")));

            var commandHandler =
                new UpdateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new UpdateUserCommand(userId, "Emile-A239", "emile-a239", "some-hash");
            var result = await commandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.UserAlreadyExists, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSaveFails_ShouldReturnSavingChangesError()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => false);

            var userId = Guid.NewGuid();
            var applicationUser = new Mock<IApplicationUser>();
            applicationUser.Setup(x => x.Id).Returns(userId);

            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(applicationUser.Object));
            applicationUserRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);

            var identityQueries = new Mock<IIdentityQueries>();
            identityQueries.Setup(x => x.GetUserByNormalizedUserName(It.IsAny<string>())).ReturnsAsync(() =>
                Maybe<UserModel>.Nothing);
            var commandHandler =
                new UpdateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new UpdateUserCommand(Guid.NewGuid(), "Emile-A239", "emile-a239", "some-hash");
            var result = await commandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.SavingChanges, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSuccess_ShouldReturnSuccessfulResult()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => true);

            var userId = Guid.NewGuid();
            var applicationUser = new Mock<IApplicationUser>();
            applicationUser.Setup(x => x.Id).Returns(userId);

            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(applicationUser.Object));
            applicationUserRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var identityQueries = new Mock<IIdentityQueries>();
            identityQueries.Setup(x => x.GetUserByNormalizedUserName(It.IsAny<string>())).ReturnsAsync(() =>
                Maybe<UserModel>.Nothing);
            var commandHandler =
                new UpdateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new UpdateUserCommand(Guid.NewGuid(), "Emile-A239", "emile-a239", "some-hash");
            var result = await commandHandler.Handle(command, default);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_GivenUserDoesNotExists_ShouldReturnUserDoesNotExistsError()
        {
            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe<IApplicationUser>.Nothing);
            var identityQueries = new Mock<IIdentityQueries>();

            var commandHandler =
                new UpdateUserCommandHandler(applicationUserRepository.Object, identityQueries.Object);

            var command = new UpdateUserCommand(Guid.NewGuid(), "Emile-A239", "emile-a239", "some-hash");
            var result = await commandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.UserDoesNotExist, result.Error.Code);
        }
    }
}