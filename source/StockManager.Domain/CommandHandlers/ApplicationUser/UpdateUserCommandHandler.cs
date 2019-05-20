using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Core.Constants;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;
using StockManager.Domain.Commands.ApplicationUser;
using StockManager.Queries.Contracts;

namespace StockManager.Domain.CommandHandlers.ApplicationUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResultWithError<ErrorData>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IIdentityQueries _identityQueries;

        public UpdateUserCommandHandler(IApplicationUserRepository applicationUserRepository, IIdentityQueries identityQueries)
        {
            this._applicationUserRepository = applicationUserRepository ??
                                              throw new ArgumentNullException(nameof(applicationUserRepository));
            this._identityQueries = identityQueries ?? throw new ArgumentNullException(nameof(identityQueries));
        }

        public async Task<ResultWithError<ErrorData>> Handle(
            UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userMaybe = await this._applicationUserRepository.FindById(request.Id, cancellationToken);
            if (userMaybe.HasNoValue)
            {
                return ResultWithError.Fail(new ErrorData(ErrorCodes.UserDoesNotExist));
            }

            var presenceCheck = await this._identityQueries.CheckForPresenceOfUserByNormalizedUserNameAndId(request.NormalizedUserName, request.Id);
            if (presenceCheck.IsPresent)
            {
                return ResultWithError.Fail(new ErrorData(ErrorCodes.UserAlreadyExists));
            }

            var user = userMaybe.Value;
            user.UpdateDetails(request.UserName, request.NormalizedUserName, request.PasswordHash);
            this._applicationUserRepository.Update(user);
            var saveResult = await this._applicationUserRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (saveResult)
            {
                return ResultWithError.Ok<ErrorData>();
            }

            return ResultWithError.Fail(new ErrorData(ErrorCodes.SavingChanges));
        }
    }
}