using System;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Domain.CommandResults.Stock;

namespace StockManager.Domain.Commands.Stock
{
    public sealed class AddBatchCommand : IRequest<Result<AddBatchCommandResult, ErrorData>>
    {
        public AddBatchCommand(Guid typeId, int quantity)
        {
            this.TypeId = typeId;
            this.Quantity = quantity;
        }

        public Guid TypeId { get; }

        public int Quantity { get; }
    }
}