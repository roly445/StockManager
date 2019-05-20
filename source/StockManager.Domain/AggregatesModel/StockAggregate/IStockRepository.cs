using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using StockManager.Core.Contracts;

namespace StockManager.Domain.AggregatesModel.StockAggregate
{
    public interface IStockRepository : IRepository<IStock>
    {
        IStock Add(IStock stock);

        void Update(IStock stock);

        Task<Maybe<IStock>> FindByTypeId(Guid typeId, CancellationToken cancellationToken);
    }
}