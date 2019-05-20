using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using StockManager.Core.Constants;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.CategoryAggregate;
using StockManager.Domain.CommandHandlers.Category;
using StockManager.Domain.Commands.Category;
using StockManager.Queries.Contracts;
using StockManager.Queries.Models;
using Xunit;

namespace StockManager.Domain.Tests.CommandHandlers.Category
{
    public class CreateCategoryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_GivenCategoryExists_ShouldReturnCategoryExistsError()
        {
            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfCategoryByName(It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(true));

            var command = new CreateCategoryCommand("Destroyer");

            var handler = new CreateCategoryCommandHandler(categoryRepository.Object, categoryQueries.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.CategoryAlreadyExists, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSaveFails_ShouldReturnSavingChangesError()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => false);
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfCategoryByName(It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(false));

            var command = new CreateCategoryCommand("Destroyer");

            var handler = new CreateCategoryCommandHandler(categoryRepository.Object, categoryQueries.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.SavingChanges, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSuccess_ShouldReturnSuccessfulResult()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => true);
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfCategoryByName(It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(false));

            var command = new CreateCategoryCommand("Destroyer");

            var handler = new CreateCategoryCommandHandler(categoryRepository.Object, categoryQueries.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value.CategoryId);
        }
    }
}