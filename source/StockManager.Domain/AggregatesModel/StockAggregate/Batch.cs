using System;
using StockManager.Core.Domain;

namespace StockManager.Domain.AggregatesModel.StockAggregate
{
    public class Batch : Entity
    {
        public Batch(Guid id, int quantity)
        {
            this.Id = id;
            this.Quantity = quantity;
        }

        private Batch()
        {
        }

        public int Quantity { get; }
    }
}