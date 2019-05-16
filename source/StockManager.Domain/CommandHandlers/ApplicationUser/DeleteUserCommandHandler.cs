using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Core.Constants;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;
using StockManager.Domain.Commands.ApplicationUser;

namespace StockManager.Domain.CommandHandlers.ApplicationUser
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResultWithError<ErrorData>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public DeleteUserCommandHandler(IApplicationUserRepository applicationUserRepository)
        {
            this._applicationUserRepository = applicationUserRepository ?? throw new ArgumentNullException(nameof(applicationUserRepository));
        }

        public async Task<ResultWithError<ErrorData>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userMaybe = await this._applicationUserRepository.FindById(request.UserId, cancellationToken);
            if (userMaybe.HasNoValue)
            {
                return ResultWithError.Fail(new ErrorData(ErrorCodes.UserDoesNotExist));
            }

            var user = userMaybe.Value;
            this._applicationUserRepository.Delete(user);
            var saveResult = await this._applicationUserRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (saveResult)
            {
                return ResultWithError.Ok<ErrorData>();
            }

            return ResultWithError.Fail(new ErrorData(ErrorCodes.SavingChanges));
        }
    }
}