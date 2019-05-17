using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Moq.Dapper;
using StockManager.Queries.ConnectionProviders;
using StockManager.Queries.TransferObjects;
using Xunit;

namespace StockManager.Queries.Tests
{
    public class IdentityQueriesTests
    {
        [Fact]
        public async Task GetUserById_UserIsInStore_ShouldReturnUserModel()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                UserName = "Jacob Keyes",
                NormalizedUserName = "jacob-keyes",
                PasswordHash = "some-hash"
            };
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<UserDto>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<UserDto>
                {
                    user
                });
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var stockManagerQueries = new IdentityQueries(dbConnectionProvider.Object);
            var userMaybe = await stockManagerQueries.GetUserById(user.Id);
            Assert.True(userMaybe.HasValue);
            Assert.Equal(user.Id, userMaybe.Value.Id);
            Assert.Equal(user.UserName, userMaybe.Value.UserName);
            Assert.Equal(user.NormalizedUserName, userMaybe.Value.NormalizedUserName);
            Assert.Equal(user.PasswordHash, userMaybe.Value.PasswordHash);
        }

        [Fact]
        public async Task GetUserById_UserIsNotInStore_ShouldReturnNothing()
        {
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<UserDto>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<UserDto>());
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var stockManagerQueries = new IdentityQueries(dbConnectionProvider.Object);
            var userMaybe = await stockManagerQueries.GetUserById(Guid.NewGuid());
            Assert.True(userMaybe.HasNoValue);
        }

        [Fact]
        public async Task GetUserByNormalizedUserName_UserIsInStore_ShouldReturnUserModel()
        {
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                UserName = "Jacob Keyes",
                NormalizedUserName = "jacob-keyes",
                PasswordHash = "some-hash"
            };
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<UserDto>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<UserDto>
                {
                    user
                });
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var stockManagerQueries = new IdentityQueries(dbConnectionProvider.Object);
            var userMaybe = await stockManagerQueries.GetUserByNormalizedUserName(user.NormalizedUserName);
            Assert.True(userMaybe.HasValue);
            Assert.Equal(user.Id, userMaybe.Value.Id);
            Assert.Equal(user.UserName, userMaybe.Value.UserName);
            Assert.Equal(user.NormalizedUserName, userMaybe.Value.NormalizedUserName);
            Assert.Equal(user.PasswordHash, userMaybe.Value.PasswordHash);
        }

        [Fact]
        public async Task GetUserByNormalizedUserName_UserIsNotInStore_ShouldReturnNothing()
        {
            var connection = new Mock<IDbConnection>();
            connection.SetupDapperAsync(c => c.QueryAsync<UserDto>(It.IsAny<string>(), null, null, null, null))
                .ReturnsAsync(() => new List<UserDto>());
            var dbConnectionProvider = new Mock<IDbConnectionProvider>();
            dbConnectionProvider.SetupGet(x => x.Connection).Returns(connection.Object);
            var stockManagerQueries = new IdentityQueries(dbConnectionProvider.Object);
            var userMaybe = await stockManagerQueries.GetUserByNormalizedUserName(string.Empty);
            Assert.True(userMaybe.HasNoValue);
        }
    }
}