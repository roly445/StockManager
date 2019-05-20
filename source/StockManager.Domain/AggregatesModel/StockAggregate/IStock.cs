using System;
using System.Collections.Generic;
using StockManager.Core.Contracts;

namespace StockManager.Domain.AggregatesModel.StockAggregate
{
    public interface IStock : IAggregateRoot, IEntity
    {
        Guid TypeId { get; }

        int Quantity { get; }

        IReadOnlyCollection<Batch> Batches { get; }

        Batch AddBatch(Guid batchId, int quantity);
    }
}