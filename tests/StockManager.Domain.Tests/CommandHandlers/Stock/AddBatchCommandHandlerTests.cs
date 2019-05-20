using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using Moq;
using StockManager.Core.Constants;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.StockAggregate;
using StockManager.Domain.CommandHandlers.Stock;
using StockManager.Domain.Commands.Stock;
using Xunit;

namespace StockManager.Domain.Tests.CommandHandlers.Stock
{
    public sealed class AddBatchCommandHandlerTests
    {
        [Fact]
        public async Task Handle_GivenSaveFails_ShouldReturnSavingChangesError()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(c => c.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => false);
            var stockRepository = new Mock<IStockRepository>();
            stockRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            stockRepository.Setup(x => x.FindByTypeId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe<IStock>.Nothing);
            stockRepository.Setup(x => x.Add(It.IsAny<IStock>()));

            var handler = new AddBatchCommandHandler(stockRepository.Object);
            var command = new AddBatchCommand(Guid.NewGuid(), 34);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCodes.SavingChanges, result.Error.Code);
        }

        [Fact]
        public async Task Handle_GivenStockDoesExist_StockShouldBeUpdated()
        {
            var repositoryCallback = false;
            var unitOfWork = new Mock<IUnitOfWork>();
            var stock = new Mock<IStock>();
            var stockRepository = new Mock<IStockRepository>();
            stockRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            stockRepository.Setup(x => x.FindByTypeId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe.From(stock.Object));
            stockRepository.Setup(x => x.Update(It.IsAny<IStock>())).Callback(() => repositoryCallback = true);

            var handler = new AddBatchCommandHandler(stockRepository.Object);
            var command = new AddBatchCommand(Guid.NewGuid(), 34);
            var result = await handler.Handle(command, default);
            Assert.True(repositoryCallback);
        }

        [Fact]
        public async Task Handle_GivenStockDoesNotExist_StockShouldBeCreated()
        {
            var repositoryCallback = false;
            var unitOfWork = new Mock<IUnitOfWork>();
            var stockRepository = new Mock<IStockRepository>();
            stockRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            stockRepository.Setup(x => x.FindByTypeId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe<IStock>.Nothing);
            stockRepository.Setup(x => x.Add(It.IsAny<IStock>())).Callback(() => repositoryCallback = true);

            var handler = new AddBatchCommandHandler(stockRepository.Object);
            var command = new AddBatchCommand(Guid.NewGuid(), 34);
            await handler.Handle(command, default);
            Assert.True(repositoryCallback);
        }

        [Fact]
        public async Task Handle_GivenSuccess_ShouldReturnSuccessfulResult()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(c => c.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => true);
            var stockRepository = new Mock<IStockRepository>();
            stockRepository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
            stockRepository.Setup(x => x.FindByTypeId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Maybe<IStock>.Nothing);
            stockRepository.Setup(x => x.Add(It.IsAny<IStock>()));

            var handler = new AddBatchCommandHandler(stockRepository.Object);
            var command = new AddBatchCommand(Guid.NewGuid(), 34);
            var result = await handler.Handle(command, default);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.NewGuid(), result.Value.BatchId);
            Assert.NotEqual(Guid.NewGuid(), result.Value.StockId);
        }
    }
}