using System;

namespace StockManager.Domain.CommandResults.Stock
{
    public class AddBatchCommandResult
    {
        public AddBatchCommandResult(Guid batchId, Guid stockId)
        {
            this.BatchId = batchId;
            this.StockId = stockId;
        }

        public Guid BatchId { get; }

        public Guid StockId { get; }
    }
}