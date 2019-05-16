﻿using System;
using System.Threading.Tasks;
using MaybeMonad;
using StockManager.Queries.Models;

namespace StockManager.Queries
{
    public interface IStockManagerQueries
    {
        Task<Maybe<UserModel>> GetUserById(Guid userId);

        Task<Maybe<UserModel>> GetUserByNormalizedUserName(string normalizedUserName);
    }
}