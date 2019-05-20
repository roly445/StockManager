using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Core.Constants;
using StockManager.Domain.AggregatesModel.StockAggregate;
using StockManager.Domain.CommandResults.Stock;
using StockManager.Domain.Commands.Stock;

namespace StockManager.Domain.CommandHandlers.Stock
{
    public class AddBatchCommandHandler : IRequestHandler<AddBatchCommand, Result<AddBatchCommandResult, ErrorData>>
    {
        private readonly IStockRepository _stockRepository;

        public AddBatchCommandHandler(IStockRepository stockRepository)
        {
            this._stockRepository = stockRepository;
        }

        public async Task<Result<AddBatchCommandResult, ErrorData>> Handle(
            AddBatchCommand request, CancellationToken cancellationToken)
        {
            var stockMaybe = await this._stockRepository.FindByTypeId(request.TypeId, cancellationToken);
            var stock = stockMaybe.HasValue ? stockMaybe.Value : new AggregatesModel.StockAggregate.Stock(Guid.NewGuid(), request.TypeId);

            var batch = stock.AddBatch(Guid.NewGuid(), request.Quantity);

            if (stockMaybe.HasValue)
            {
                this._stockRepository.Update(stock);
            }
            else
            {
                this._stockRepository.Add(stock);
            }

            var saveResult = await this._stockRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (saveResult)
            {
                return Result.Ok<AddBatchCommandResult, ErrorData>(new AddBatchCommandResult(batch.Id, stock.Id));
            }

            return Result.Fail<AddBatchCommandResult, ErrorData>(
                new ErrorData(ErrorCodes.SavingChanges));
        }
    }
}