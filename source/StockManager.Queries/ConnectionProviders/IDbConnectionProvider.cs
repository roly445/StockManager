using System.Data;

namespace StockManager.Queries.ConnectionProviders
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}