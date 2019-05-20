using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using Microsoft.EntityFrameworkCore;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.StockAggregate;

namespace StockManager.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly StockManagerContext _context;

        public StockRepository(StockManagerContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => this._context;

        public IStock Add(IStock stock)
        {
            var obj = stock as Stock;
            if (obj == null)
            {
                throw new ArgumentException("IStock");
            }

            return this._context.Stocks.Add(obj).Entity;
        }

        public void Update(IStock stock)
        {
            this._context.Entry(stock).State = EntityState.Modified;
        }

        public async Task<Maybe<IStock>> FindByTypeId(Guid typeId, CancellationToken cancellationToken)
        {
            var stock = await this._context.Stocks.SingleOrDefaultAsync(x => x.TypeId == typeId, cancellationToken);
            await this.LoadCollections(stock);
            return stock;
        }

        private async Task LoadCollections(Stock stock)
        {
            if (stock != null)
            {
                await this._context.Entry(stock).ReloadAsync();
                await this._context.Entry(stock)
                    .Collection(x => x.Batches)
                    .LoadAsync();
            }
        }
    }
}