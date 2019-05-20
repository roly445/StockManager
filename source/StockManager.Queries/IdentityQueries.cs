using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MaybeMonad;
using StockManager.Queries.ConnectionProviders;
using StockManager.Queries.Contracts;
using StockManager.Queries.Models;
using StockManager.Queries.TransferObjects;

namespace StockManager.Queries
{
    public sealed class IdentityQueries : IIdentityQueries
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public IdentityQueries(IDbConnectionProvider dbConnectionProvider)
        {
            this._dbConnectionProvider =
                dbConnectionProvider ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public async Task<Maybe<UserModel>> GetUserById(Guid userId)
        {
            using (var dbConnection = this._dbConnectionProvider.Connection)
            {
                dbConnection.Open();

                var res = await dbConnection.QueryAsync<UserDto>(
                    "identity.uspGetUserById",
                    new { userId }, commandType: CommandType.StoredProcedure);
                var userDtos = res as UserDto[] ?? res.ToArray();
                if (userDtos.Count() != 1)
                {
                    return Maybe<UserModel>.Nothing;
                }

                var obj = userDtos.Single();
                return Maybe.From(new UserModel(obj.Id, obj.UserName, obj.NormalizedUserName, obj.PasswordHash));
            }
        }

        public async Task<Maybe<UserModel>> GetUserByNormalizedUserName(string normalizedUserName)
        {
            using (var dbConnection = this._dbConnectionProvider.Connection)
            {
                dbConnection.Open();

                var res = await dbConnection.QueryAsync<UserDto>(
                    "identity.uspGetUserByNormalizedUserName",
                    new { normalizedUserName }, commandType: CommandType.StoredProcedure);
                var userDtos = res as UserDto[] ?? res.ToArray();
                if (userDtos.Count() != 1)
                {
                    return Maybe<UserModel>.Nothing;
                }

                var obj = userDtos.Single();
                return Maybe.From(new UserModel(obj.Id, obj.UserName, obj.NormalizedUserName, obj.PasswordHash));
            }
        }

        public async Task<StatusCheckModel> CheckForPresenceOfUserByNormalizedUserName(string normalizedUserName)
        {
            using (var dbConnection = this._dbConnectionProvider.Connection)
            {
                dbConnection.Open();

                var res = await dbConnection.QueryAsync<PresenceCheckDto<Guid>>(
                    "identity.uspCheckForPresenceOfUserByNormalizedUserName",
                    new {normalizedUserName}, commandType: CommandType.StoredProcedure);
                var dtos = res as PresenceCheckDto<Guid>[] ?? res.ToArray();
                return new StatusCheckModel(dtos.Length > 0);
            }
        }

        public async Task<StatusCheckModel> CheckForPresenceOfUserByNormalizedUserNameAndId(string normalizedUserName, Guid userId)
        {
            using (var dbConnection = this._dbConnectionProvider.Connection)
            {
                dbConnection.Open();

                var res = await dbConnection.QueryAsync<PresenceCheckDto<Guid>>(
                    "identity.uspCheckForPresenceOfUserByNormalizedUserNameAndId",
                    new {userId, normalizedUserName}, commandType: CommandType.StoredProcedure);
                var dtos = res as PresenceCheckDto<Guid>[] ?? res.ToArray();
                return new StatusCheckModel(dtos.Length > 0);
            }
        }
    }
}