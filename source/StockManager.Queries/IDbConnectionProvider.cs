using System.Data;

namespace StockManager.Queries
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }
}