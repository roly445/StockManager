using System;
using StockManager.Domain.AggregatesModel.StockAggregate;
using Xunit;

namespace StockManager.Domain.Tests.AggregatesModel.StockAggregate
{
    public class BatchTests
    {
        [Fact]
        public void Ctor_AllPropertiesShouldBeSet()
        {
            var batchId = Guid.NewGuid();
            var batch = new Batch(batchId, 1050);
            Assert.Equal(batchId, batch.Id);
            Assert.Equal(1050, batch.Quantity);
        }
    }
}