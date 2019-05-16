using StockManager.Core.Contracts;

namespace StockManager.Domain.AggregatesModel.ApplicationUserAggregate
{
    public interface IApplicationUser : IAggregateRoot, IEntity
    {
        string UserName { get; }

        string NormalizedUserName { get; }

        string PasswordHash { get; }

        void UpdateDetails(string userName, string normalizedUserName, string passwordHash);
    }
}