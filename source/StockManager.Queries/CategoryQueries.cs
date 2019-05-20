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
    public sealed class CategoryQueries : ICategoryQueries
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public CategoryQueries(IDbConnectionProvider dbConnectionProvider)
        {
            this._dbConnectionProvider =
                dbConnectionProvider ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public async Task<StatusCheckModel> CheckForPresenceOfCategoryByName(string name)
        {
            using (var dbConnection = this._dbConnectionProvider.Connection)
            {
                dbConnection.Open();

                var res = await dbConnection.QueryAsync<PresenceCheckDto<Guid>>(
                    "stock.uspCheckForPresenceOfCategoryByName",
                    new {name}, commandType: CommandType.StoredProcedure);
                var dtos = res as PresenceCheckDto<Guid>[] ?? res.ToArray();
                return new StatusCheckModel(dtos.Length > 0);
            }
        }

        public async Task<StatusCheckModel> CheckForPresenceOfTypeByCategoryAndName(Guid categoryId, string name)
        {
            using (var dbConnection = this._dbConnectionProvider.Connection)
            {
                dbConnection.Open();

                var res = await dbConnection.QueryAsync<PresenceCheckDto<Guid>>(
                    "stock.CheckForPresenceOfTypeByCategoryAndName",
                    new {categoryId, name}, commandType: CommandType.StoredProcedure);
                var dtos = res as PresenceCheckDto<Guid>[] ?? res.ToArray();
                return new StatusCheckModel(dtos.Length > 0);
            }
        }
    }
}