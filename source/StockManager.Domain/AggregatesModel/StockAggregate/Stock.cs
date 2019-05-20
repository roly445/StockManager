using System;
using System.Collections.Generic;
using StockManager.Core.Domain;

namespace StockManager.Domain.AggregatesModel.StockAggregate
{
    public class Stock : Entity, IStock
    {
        private readonly List<Batch> _batches;

        public Stock(Guid id, Guid typeId)
        {
            this.Id = id;
            this.TypeId = typeId;
            this.Quantity = 0;
            this._batches = new List<Batch>();
        }

        private Stock()
        {
            this._batches = new List<Batch>();
        }

        public Guid TypeId { get; private set; }

        public int Quantity { get; private set; }

        public IReadOnlyCollection<Batch> Batches => this._batches.AsReadOnly();

        public Batch AddBatch(Guid batchId, int quantity)
        {
            var batch = new Batch(batchId, quantity);
            this._batches.Add(batch);
            this.Quantity += quantity;
            return batch;
        }
    }
}