using System;
using System.Threading;
using System.Threading.Tasks;
using MaybeMonad;
using Microsoft.EntityFrameworkCore;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;

namespace StockManager.Infrastructure.Repositories
{
    public sealed class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly StockManagerContext _context;

        public ApplicationUserRepository(StockManagerContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => this._context;

        public IApplicationUser Add(IApplicationUser applicationUser)
        {
            var obj = applicationUser as ApplicationUser;
            if (obj == null)
            {
                throw new ArgumentException("IApplicationUser");
            }

            return this._context.ApplicationUsers.Add(obj).Entity;
        }

        public void Update(IApplicationUser user)
        {
            this._context.Entry(user).State = EntityState.Modified;
        }

        public void Delete(IApplicationUser user)
        {
            this._context.Entry(user).State = EntityState.Deleted;
        }

        public async Task<Maybe<IApplicationUser>> FindById(Guid id, CancellationToken cancellationToken)
        {
            var appUser = await this._context.ApplicationUsers.FindAsync(id, cancellationToken);
            await this.LoadCollections(appUser);
            return appUser;
        }

        private async Task LoadCollections(IApplicationUser appUser)
        {
            if (appUser != null)
            {
                await this._context.Entry(appUser).ReloadAsync();
            }
        }
    }
}