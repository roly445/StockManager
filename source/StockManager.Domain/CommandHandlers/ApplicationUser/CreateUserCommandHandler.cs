using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ResultMonad;
using StockManager.Core;
using StockManager.Core.Constants;
using StockManager.Domain.AggregatesModel.ApplicationUserAggregate;
using StockManager.Domain.CommandResults.ApplicationUser;
using StockManager.Domain.Commands.ApplicationUser;
using StockManager.Queries.Contracts;

namespace StockManager.Domain.CommandHandlers.ApplicationUser
{
    public class
        CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResult, ErrorData>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IIdentityQueries _identityQueries;

        public CreateUserCommandHandler(IApplicationUserRepository applicationUserRepository, IIdentityQueries identityQueries)
        {
            this._applicationUserRepository = applicationUserRepository ?? throw new ArgumentNullException(nameof(applicationUserRepository));
            this._identityQueries = identityQueries ?? throw new ArgumentNullException(nameof(identityQueries));
        }

        public async Task<Result<CreateUserCommandResult, ErrorData>> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var userMaybe = await this._identityQueries.GetUserByNormalizedUserName(request.NormalizedUserName);
            if (userMaybe.HasValue)
            {
                return Result.Fail<CreateUserCommandResult, ErrorData>(
                    new ErrorData(ErrorCodes.UserAlreadyExists));
            }

            var applicationUser = new AggregatesModel.ApplicationUserAggregate.ApplicationUser(
                Guid.NewGuid(), request.UserName, request.NormalizedUserName, request.PasswordHash);

            this._applicationUserRepository.Add(applicationUser);
            var saveResult = await this._applicationUserRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            if (saveResult)
            {
                return Result.Ok<CreateUserCommandResult, ErrorData>(new CreateUserCommandResult(applicationUser.Id));
            }

            return Result.Fail<CreateUserCommandResult, ErrorData>(
                new ErrorData(ErrorCodes.SavingChanges));
        }
    }
}