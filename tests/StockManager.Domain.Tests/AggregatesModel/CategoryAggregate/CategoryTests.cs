using System;
using System.Linq;
using StockManager.Domain.AggregatesModel.CategoryAggregate;
using Xunit;
using Type = StockManager.Domain.AggregatesModel.CategoryAggregate.Type;

namespace StockManager.Domain.Tests.AggregatesModel.CategoryAggregate
{
    public class CategoryTests
    {
        [Fact]
        public void Ctor_AllPropertiesShouldBeSet()
        {
            var categoryId = Guid.NewGuid();
            var category = new Category(categoryId, "Carriers");
            Assert.Equal(categoryId, category.Id);
            Assert.Equal("Carriers", category.Name);
            Assert.NotNull(category.Types);
        }

        [Fact]
        public void AddType_ShouldAddTypeToListAddReturnTheType()
        {
            var categoryId = Guid.NewGuid();
            var typeId = Guid.NewGuid();
            var category = new Category(categoryId, "Carriers");
            var type = category.AddType(typeId, "UNSC Magellan ");
            Assert.Equal(1, category.Types.Count);
            Assert.Equal(type, category.Types.First());
            Assert.Equal(typeId, category.Types.First().Id);
            Assert.Equal("UNSC Magellan ", category.Types.First().Name);
        }
    }
}