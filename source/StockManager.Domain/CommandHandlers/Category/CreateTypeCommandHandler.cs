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
    public class
        CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, Result<CreateTypeCommandResult, ErrorData>>
    {
        private readonly ICategoryQueries _categoryQueries;
        private readonly ICategoryRepository _categoryRepository;

        public CreateTypeCommandHandler(ICategoryQueries categoryQueries, ICategoryRepository categoryRepository)
        {
            this._categoryQueries = categoryQueries ?? throw new ArgumentNullException(nameof(categoryQueries));
            this._categoryRepository =
                categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Result<CreateTypeCommandResult, ErrorData>> Handle(
            CreateTypeCommand request, CancellationToken cancellationToken)
        {
            var presenceCheck =
                await this._categoryQueries.CheckForPresenceOfTypeByCategoryAndName(request.CategoryId, request.Name);
            if (presenceCheck.IsPresent)
            {
                return Result.Fail<CreateTypeCommandResult, ErrorData>(new ErrorData(ErrorCodes.TypeAlreadyExists));
            }

            var categoryMaybe = await this._categoryRepository.FindById(request.CategoryId, cancellationToken);
            if (categoryMaybe.HasNoValue)
            {
                return Result.Fail<CreateTypeCommandResult, ErrorData>(new ErrorData(ErrorCodes.CategoryDoesNotExists));
            }

            var category = categoryMaybe.Value;
            var type = category.AddType(Guid.NewGuid(), request.Name);
            this._categoryRepository.Update(category);
            var saveResult = await this._categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (saveResult)
            {
                return Result.Ok<CreateTypeCommandResult, ErrorData>(new CreateTypeCommandResult(type.Id));
            }

            return Result.Fail<CreateTypeCommandResult, ErrorData>(
                new ErrorData(ErrorCodes.SavingChanges));
        }
    }
}