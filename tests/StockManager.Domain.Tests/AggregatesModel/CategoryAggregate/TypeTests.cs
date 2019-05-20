using System;
using Xunit;
using Type = StockManager.Domain.AggregatesModel.CategoryAggregate.Type;

namespace StockManager.Domain.Tests.AggregatesModel.CategoryAggregate
{
    public class TypeTests
    {
        [Fact]
        public void Ctor_AllPropertiesShouldBeSet()
        {
            var typeId = Guid.NewGuid();
            var type = new Type(typeId, "UNSC All Under Heaven");
            Assert.Equal(typeId, type.Id);
            Assert.Equal("UNSC All Under Heaven", type.Name);
        }
    }
}