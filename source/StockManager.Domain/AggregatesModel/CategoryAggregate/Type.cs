using System;
using StockManager.Core.Domain;

namespace StockManager.Domain.AggregatesModel.CategoryAggregate
{
    public class Type : Entity
    {
        public Type(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        private Type()
        {
        }

        public string Name { get; private set; }
    }
}