using System;
using System.Collections.Generic;
using StockManager.Core.Domain;

namespace StockManager.Domain.AggregatesModel.CategoryAggregate
{
    public class Category : Entity, ICategory
    {
        private readonly List<Type> _types;

        public Category(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
            this._types = new List<Type>();
        }

        private Category()
        {
            this._types = new List<Type>();
        }

        public string Name { get; private set; }

        public IReadOnlyCollection<Type> Types => this._types.AsReadOnly();

        public Type AddType(Guid id, string name)
        {
            var type = new Type(id, name);
            this._types.Add(type);
            return type;
        }
    }
}