using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Moq.Dapper;
using StockManager.Queries.ConnectionProviders;
using StockManager.Queries.TransferObjects;
using Xunit;

namespace StockManager.Queries.Tests
{
    public sealed class CategoryQueriesTests
    {
        [Fact]
        public async Task CheckForPresenceOfCategoryByName_CategoryIsInStore_ShouldReturnTrue()
        {
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<PresenceCheckDto<Guid>>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<PresenceCheckDto<Guid>>
                {
                    new PresenceCheckDto<Guid>
                    {
                        PresenceIdentifier = Guid.NewGuid(),
                    },
                });
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var categoryQueries = new CategoryQueries(dbConnectionProvider.Object);
            var statusCheck = await categoryQueries.CheckForPresenceOfCategoryByName(new string('*', 5));
            Assert.True(statusCheck.IsPresent);
        }

        [Fact]
        public async Task CheckForPresenceOfCategoryByName_CategoryIsNotInStore_ShouldReturnFalse()
        {
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<PresenceCheckDto<Guid>>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<PresenceCheckDto<Guid>>());
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var categoryQueries = new CategoryQueries(dbConnectionProvider.Object);
            var statusCheck = await categoryQueries.CheckForPresenceOfCategoryByName(new string('*', 5));
            Assert.False(statusCheck.IsPresent);
        }

        [Fact]
        public async Task CheckForPresenceOfTypeByCategoryAndName_CategoryIsInStore_ShouldReturnTrue()
        {
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<PresenceCheckDto<Guid>>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<PresenceCheckDto<Guid>>
                {
                    new PresenceCheckDto<Guid>
                    {
                        PresenceIdentifier = Guid.NewGuid(),
                    },
                });
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var categoryQueries = new CategoryQueries(dbConnectionProvider.Object);
            var statusCheck = await categoryQueries.CheckForPresenceOfTypeByCategoryAndName(Guid.NewGuid(), new string('*', 5));
            Assert.True(statusCheck.IsPresent);
        }

        [Fact]
        public async Task CheckForPresenceOfTypeByCategoryAndName_CategoryIsNotInStore_ShouldReturnFalse()
        {
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<PresenceCheckDto<Guid>>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<PresenceCheckDto<Guid>>());
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var categoryQueries = new CategoryQueries(dbConnectionProvider.Object);
            var statusCheck = await categoryQueries.CheckForPresenceOfTypeByCategoryAndName(Guid.NewGuid(), new string('*', 5));
            Assert.False(statusCheck.IsPresent);
        }
    }
}
