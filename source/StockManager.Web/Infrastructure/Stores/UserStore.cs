using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using StockManager.Domain.Commands.ApplicationUser;
using StockManager.Queries;

namespace StockManager.Web.Infrastructure.Stores
{
    public sealed class UserStore :
        IUserStore<AppUser>,
        IUserPasswordStore<AppUser>
    {
        private readonly IMediator _mediator;
        private readonly IStockManagerQueries _stockManagerQueries;

        public UserStore(IMediator mediator, IStockManagerQueries stockManagerQueries)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this._stockManagerQueries =
                stockManagerQueries ?? throw new ArgumentNullException(nameof(stockManagerQueries));
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.UpdatePasswordHash(passwordHash);
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            user.UpdateUserName(userName);
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UpdateNormalizedUserName(normalizedName);
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(user.UserName, user.NormalizedUserName, user.PasswordHash);
            var result = await this._mediator.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                user.UpdateId(result.Value.UserId);
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand(user.Id, user.UserName, user.NormalizedUserName, user.PasswordHash);
            var result = await this._mediator.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(user.Id);
            var result = await this._mediator.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed();
        }

        public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var userMaybe = await this._stockManagerQueries.GetUserById(Guid.Parse(userId));
            if (userMaybe.HasNoValue)
            {
                return null;
            }

            var user = userMaybe.Value;
            return new AppUser(user.Id, user.NormalizedUserName, user.UserName, user.PasswordHash);
        }

        public async Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var userMaybe = await this._stockManagerQueries.GetUserByNormalizedUserName(normalizedUserName);
            if (userMaybe.HasNoValue)
            {
                return null;
            }

            var user = userMaybe.Value;
            return new AppUser(user.Id, user.NormalizedUserName, user.UserName, user.PasswordHash);
        }
    }
}