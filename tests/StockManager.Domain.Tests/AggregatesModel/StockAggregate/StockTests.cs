using System;
using System.Linq;
using StockManager.Domain.AggregatesModel.StockAggregate;
using Xunit;

namespace StockManager.Domain.Tests.AggregatesModel.StockAggregate
{
    public class StockTests
    {
        [Fact]
        public void AddType_ShouldAddTypeToListAddReturnTheType()
        {
            var stockId = Guid.NewGuid();
            var typeId = Guid.NewGuid();
            var batchId = Guid.NewGuid();
            var stock = new Stock(stockId, typeId);
            var batch = stock.AddBatch(batchId, 45);
            Assert.Equal(1, stock.Batches.Count);
            Assert.Equal(batch, stock.Batches.First());
            Assert.Equal(batchId, stock.Batches.First().Id);
            Assert.Equal(45, stock.Quantity);
        }

        [Fact]
        public void Ctor_AllPropertiesShouldBeSet()
        {
            var stockId = Guid.NewGuid();
            var typeId = Guid.NewGuid();
            var stock = new Stock(stockId, typeId);
            Assert.Equal(stockId, stock.Id);
            Assert.Equal(typeId, stock.TypeId);
            Assert.Equal(0, stock.Quantity);
            Assert.NotNull(stock.Batches);
        }
    }
}