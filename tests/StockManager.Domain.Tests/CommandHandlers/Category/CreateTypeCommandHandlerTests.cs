using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using Moq;
using StockManager.Core.Constants;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.CategoryAggregate;
using StockManager.Domain.CommandHandlers.Category;
using StockManager.Domain.Commands.Category;
using StockManager.Queries.Contracts;
using StockManager.Queries.Models;
using Xunit;
using Type = StockManager.Domain.AggregatesModel.CategoryAggregate.Type;

namespace StockManager.Domain.Tests.CommandHandlers.Category
{
    public class CreateTypeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_GivenCategoryDoesNotExists_ShouldReturnCategoryDoesNotExistsError()
        {
            var categoryId = Guid.NewGuid();
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe<ICategory>.Nothing);
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfTypeByCategoryAndName(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(false));

            var command = new CreateTypeCommand(categoryId, "UNSC Agincourt");

            var handler = new CreateTypeCommandHandler(categoryQueries.Object, categoryRepository.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.CategoryDoesNotExists, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSaveFails_ShouldReturnSavingChangesError()
        {
            var categoryId = Guid.NewGuid();
            var category = new Mock<ICategory>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => false);
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            categoryRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(category.Object));
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfTypeByCategoryAndName(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(false));

            var command = new CreateTypeCommand(categoryId, "UNSC Agincourt");

            var handler = new CreateTypeCommandHandler(categoryQueries.Object, categoryRepository.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.SavingChanges, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenSuccess_ShouldReturnSuccessfulResult()
        {
            var categoryId = Guid.NewGuid();
            var typeId = Guid.NewGuid();
            var type = new Type(typeId, "UNSC Agincourt");
            var category = new Mock<ICategory>();
            category.Setup(x => x.AddType(It.IsAny<Guid>(), It.IsAny<string>())).Returns(type);
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => true);
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            categoryRepository.Setup(x => x.FindById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(category.Object));
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfTypeByCategoryAndName(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(false));

            var command = new CreateTypeCommand(categoryId, "UNSC Agincourt");

            var handler = new CreateTypeCommandHandler(categoryQueries.Object, categoryRepository.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsSuccess);
            Assert.Equal(typeId, result.Value.TypeId);
        }

        [Fact]
        public async Task Handle_GivenTypeExists_ShouldReturnTypeExistsError()
        {
            var categoryId = Guid.NewGuid();
            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryQueries = new Mock<ICategoryQueries>();

            categoryQueries.Setup(x => x.CheckForPresenceOfTypeByCategoryAndName(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new StatusCheckModel(true));

            var command = new CreateTypeCommand(categoryId, "UNSC Agincourt");

            var handler = new CreateTypeCommandHandler(categoryQueries.Object, categoryRepository.Object);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.TypeAlreadyExists, result.Error.Code);
        }
    }
}