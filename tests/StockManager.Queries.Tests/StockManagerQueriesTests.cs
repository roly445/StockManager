using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Moq.Dapper;
using StockManager.Queries.TransferObjects;
using Xunit;

namespace StockManager.Queries.Tests
{
    public class StockManagerQueriesTests
    {
        [Fact]
        public async Task GetUserById_UserIsInStore_ShouldReturnUserModel()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                UserName = "Jacob Keyes",
                NormalizedUserName = "jacob-keyes",
                PasswordHash = "some-hash",
            };
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<UserDto>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<UserDto>
                {
                    user,
                });
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var stockManagerQueries = new StockManagerQueries(dbConnectionProvider.Object);
            var userMaybe = await stockManagerQueries.GetUserById(user.Id);
            Assert.True(userMaybe.HasValue);
            Assert.Equal(user.Id, userMaybe.Value.Id);
        }
    }
}