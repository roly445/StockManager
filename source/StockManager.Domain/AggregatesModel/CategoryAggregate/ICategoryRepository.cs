using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using StockManager.Core.Contracts;

namespace StockManager.Domain.AggregatesModel.CategoryAggregate
{
    public interface ICategoryRepository : IRepository<ICategory>
    {
        ICategory Add(ICategory category);

        void Update(ICategory category);

        Task<Maybe<ICategory>> FindById(Guid id, CancellationToken cancellationToken);
    }
}