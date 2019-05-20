using System;
using System.Threading.Tasks;
using MaybeMonad;
using StockManager.Queries.Models;

namespace StockManager.Queries.Contracts
{
    public interface IIdentityQueries
    {
        Task<Maybe<UserModel>> GetUserById(Guid userId);

        Task<Maybe<UserModel>> GetUserByNormalizedUserName(string normalizedUserName);
        Task<StatusCheckModel> CheckForPresenceOfUserByNormalizedUserName(string normalizedUserName);
        Task<StatusCheckModel> CheckForPresenceOfUserByNormalizedUserNameAndId(string normalizedUserName, Guid userId);
    }
}