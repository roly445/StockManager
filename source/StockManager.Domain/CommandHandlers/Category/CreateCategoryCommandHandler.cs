using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Core.Constants;
using StockManager.Domain.AggregatesModel.CategoryAggregate;
using StockManager.Domain.CommandResults.Category;
using StockManager.Domain.Commands.Category;
using StockManager.Queries.Contracts;

namespace StockManager.Domain.CommandHandlers.Category
{
    public sealed class
        CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand,
            Result<CreateCategoryCommandResult, ErrorData>>
    {
        private readonly ICategoryQueries _categoryQueries;
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ICategoryQueries categoryQueries)
        {
            this._categoryRepository =
                categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            this._categoryQueries = categoryQueries ?? throw new ArgumentNullException(nameof(categoryQueries));
        }

        public async Task<Result<CreateCategoryCommandResult, ErrorData>> Handle(
            CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var presenceCheck = await this._categoryQueries.CheckForPresenceOfCategoryByName(request.Name);
            if (presenceCheck.IsPresent)
            {
                return Result.Fail<CreateCategoryCommandResult, ErrorData>(
                    new ErrorData(ErrorCodes.CategoryAlreadyExists));
            }

            var category = new AggregatesModel.CategoryAggregate.Category(Guid.NewGuid(), request.Name);
            this._categoryRepository.Add(category);
            var saveResult = await this._categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (saveResult)
            {
                return Result.Ok<CreateCategoryCommandResult, ErrorData>(new CreateCategoryCommandResult(category.Id));
            }

            return Result.Fail<CreateCategoryCommandResult, ErrorData>(
                new ErrorData(ErrorCodes.SavingChanges));
        }
    }
}