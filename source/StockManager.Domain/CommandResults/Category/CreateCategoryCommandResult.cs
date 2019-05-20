using System;

namespace StockManager.Domain.CommandResults.Category
{
    public sealed class CreateCategoryCommandResult
    {
        public CreateCategoryCommandResult(Guid categoryId)
        {
            this.CategoryId = categoryId;
        }

        public Guid CategoryId { get; }
    }
}