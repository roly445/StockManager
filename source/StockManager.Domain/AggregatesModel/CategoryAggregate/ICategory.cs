using System;
using System.Collections.Generic;
using System.Text;
using StockManager.Core.Contracts;

namespace StockManager.Domain.AggregatesModel.CategoryAggregate
{
    public interface ICategory : IAggregateRoot, IEntity
    {
        string Name { get; }

        IReadOnlyCollection<Type> Types { get; }

        Type AddType(Guid id, string name);
    }
}