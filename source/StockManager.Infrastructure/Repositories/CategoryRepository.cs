using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using Microsoft.EntityFrameworkCore;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.CategoryAggregate;

namespace StockManager.Infrastructure.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly StockManagerContext _context;

        public CategoryRepository(StockManagerContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => this._context;

        public ICategory Add(ICategory category)
        {
            var obj = category as Category;
            if (obj == null)
            {
                throw new ArgumentException("ICategory");
            }

            return this._context.Categories.Add(obj).Entity;
        }

        public void Update(ICategory category)
        {
            this._context.Entry(category).State = EntityState.Modified;
        }

        public async Task<Maybe<ICategory>> FindById(Guid id, CancellationToken cancellationToken)
        {
            var category = await this._context.Categories.FindAsync(id, cancellationToken);
            await this.LoadCollections(category);
            return category;
        }

        private async Task LoadCollections(Category category)
        {
            if (category != null)
            {
                await this._context.Entry(category).ReloadAsync();
                await this._context.Entry(category)
                    .Collection(x => x.Types)
                    .LoadAsync();
            }
        }
    }
}