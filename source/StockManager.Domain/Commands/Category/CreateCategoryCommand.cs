using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Domain.CommandResults.Category;

namespace StockManager.Domain.Commands.Category
{
    public sealed class CreateCategoryCommand : IRequest<Result<CreateCategoryCommandResult, ErrorData>>
    {
        public CreateCategoryCommand(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}