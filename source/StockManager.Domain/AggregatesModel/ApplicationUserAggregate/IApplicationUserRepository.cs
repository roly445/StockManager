using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using StockManager.Core.Contracts;

namespace StockManager.Domain.AggregatesModel.ApplicationUserAggregate
{
    public interface IApplicationUserRepository : IRepository<IApplicationUser>
    {
        IApplicationUser Add(IApplicationUser applicationUser);

        void Update(IApplicationUser user);

        void Delete(IApplicationUser user);

        Task<Maybe<IApplicationUser>> FindById(Guid requestId, CancellationToken cancellationToken);
    }
}