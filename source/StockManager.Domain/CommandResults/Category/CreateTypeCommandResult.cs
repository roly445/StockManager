using System;

namespace StockManager.Domain.CommandResults.Category
{
    public sealed class CreateTypeCommandResult
    {
        public CreateTypeCommandResult(Guid typeId)
        {
            this.TypeId = typeId;
        }

        public Guid TypeId { get; }
    }
}