using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManager.Core.Contracts;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;
using StockManager.Domain.AggregatesModel.CategoryAggregate;
using StockManager.Domain.AggregatesModel.StockAggregate;
using Type = StockManager.Domain.AggregatesModel.CategoryAggregate.Type;

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

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.SaveChangesAsync(cancellationToken);
            await this._mediator.DispatchDomainEventsAsync(this);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(this.ConfigureApplicationUser);

            modelBuilder.Entity<Category>(this.ConfigureCategory);
            modelBuilder.Entity<Domain.AggregatesModel.CategoryAggregate.Type>(this.ConfigureType);

            modelBuilder.Entity<Stock>(this.ConfigureStock);
            modelBuilder.Entity<Batch>(this.ConfigureBatch);
        }

        private void ConfigureApplicationUser(EntityTypeBuilder<ApplicationUser> config)
        {
            config.ToTable("ApplicationUser", "Identity");
            config.HasKey(entity => entity.Id);

            config.Ignore(b => b.DomainEvents);
        }

        private void ConfigureCategory(EntityTypeBuilder<Category> config)
        {
            config.ToTable("Category", "Stock");
            config.HasKey(entity => entity.Id);

            var navigation = config.Metadata.FindNavigation(nameof(Category.Types));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            config.Ignore(b => b.DomainEvents);
        }

        private void ConfigureType(EntityTypeBuilder<Type> config)
        {
            config.ToTable("Type", "Stock");
            config.HasKey(entity => entity.Id);

            config.Ignore(b => b.DomainEvents);
        }

        private void ConfigureStock(EntityTypeBuilder<Stock> config)
        {
            config.ToTable("Stock", "Stock");
            config.HasKey(entity => entity.Id);

            var navigation = config.Metadata.FindNavigation(nameof(Stock.Batches));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            config.Ignore(b => b.DomainEvents);
        }

        private void ConfigureBatch(EntityTypeBuilder<Batch> config)
        {
            config.ToTable("Batch", "Stock");
            config.HasKey(entity => entity.Id);

            config.Ignore(b => b.DomainEvents);
        }
    }
}