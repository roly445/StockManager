using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;

namespace StockManager.Infrastructure
{
    public sealed class StockManagerContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public StockManagerContext(DbContextOptions<StockManagerContext> options, IMediator mediator)
            : base(options)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.SaveChangesAsync(cancellationToken);
            await this._mediator.DispatchDomainEventsAsync(this);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(this.ConfigureApplicationUser);
        }

        private void ConfigureApplicationUser(EntityTypeBuilder<ApplicationUser> config)
        {
            config.ToTable("ApplicationUser", "Identity");
            config.HasKey(entity => entity.Id);

            config.Ignore(b => b.DomainEvents);
        }
    }
}