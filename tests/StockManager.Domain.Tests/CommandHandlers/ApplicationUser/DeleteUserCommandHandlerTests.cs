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
using Xunit;

namespace StockManager.Domain.Tests.CommandHandlers.ApplicationUser
{
    public class DeleteUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_GivenSaveFails_ShouldReturnSavingChangesError()
        {
            var applicationUser = new Mock<IApplicationUser>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => false);
            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(applicationUser.Object));
            applicationUserRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var commandHandler =
                new DeleteUserCommandHandler(applicationUserRepository.Object);

            var command = new DeleteUserCommand(Guid.NewGuid());
            var result = await commandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.SavingChanges, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSuccess_ShouldReturnSuccessfulResult()
        {
            var applicationUser = new Mock<IApplicationUser>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => true);
            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(applicationUser.Object));
            applicationUserRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var commandHandler =
                new DeleteUserCommandHandler(applicationUserRepository.Object);

            var command = new DeleteUserCommand(Guid.NewGuid());
            var result = await commandHandler.Handle(command, default);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_GivenUserDoesNotExists_ShouldReturnUserDoesNotExistsError()
        {
            var applicationUserRepository = new Mock<IApplicationUserRepository>();
            applicationUserRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe<IApplicationUser>.Nothing);
            var createUserCommandHandler =
                new DeleteUserCommandHandler(applicationUserRepository.Object);

            var command = new DeleteUserCommand(Guid.NewGuid());
            var result = await createUserCommandHandler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.UserDoesNotExist, result.Error.Code);
        }
    }
}