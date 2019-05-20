using System;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Domain.CommandResults.Category;

namespace StockManager.Domain.Commands.Category
{
    public sealed class CreateTypeCommand : IRequest<Result<CreateTypeCommandResult, ErrorData>>
    {
        public CreateTypeCommand(Guid categoryId, string name)
        {
            this.CategoryId = categoryId;
            this.Name = name;
        }

        public Guid CategoryId { get; }

        public string Name { get; }
    }
}