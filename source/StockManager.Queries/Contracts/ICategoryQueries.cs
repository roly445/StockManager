using System;
using System.Threading.Tasks;
using MaybeMonad;
using StockManager.Queries.Models;

namespace StockManager.Queries.Contracts
{
    public interface ICategoryQueries
    {
        Task<StatusCheckModel> CheckForPresenceOfCategoryByName(string name);

        Task<StatusCheckModel> CheckForPresenceOfTypeByCategoryAndName(Guid categoryId, string name);
    }
}