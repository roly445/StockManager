using System;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;
using Xunit;

namespace StockManager.Domain.Tests.AggregatesModel.ApplicationUserAggregate
{
    public class ApplicationUserTests
    {
        [Fact]
        public void Ctor_AllPropertiesShouldBeSet()
        {
            var userId = Guid.NewGuid();
            var applicationUser = new ApplicationUser(userId, "ALICE-130", "alice-130", "some-hash");
            Assert.Equal(userId, applicationUser.Id);
            Assert.Equal("ALICE-130", applicationUser.UserName);
            Assert.Equal("alice-130", applicationUser.NormalizedUserName);
            Assert.Equal("some-hash", applicationUser.PasswordHash);
        }

        [Fact]
        public void UpdateDetails_AllPropertiesShouldBeUpdated()
        {
            var userId = Guid.NewGuid();
            var applicationUser = new ApplicationUser(userId, "ALICE-130", "alice-130", "some-hash");
            applicationUser.UpdateDetails("JUN-A266", "jun-a266", "new-hash");
            Assert.Equal(userId, applicationUser.Id);
            Assert.Equal("JUN-A266", applicationUser.UserName);
            Assert.Equal("jun-a266", applicationUser.NormalizedUserName);
            Assert.Equal("new-hash", applicationUser.PasswordHash);
        }
    }
}