using System;

namespace StockManager.Core.Exceptions
{
    public class StockManagerException : Exception
    {
        public StockManagerException()
        {
        }

        public StockManagerException(string message)
            : base(message)
        {
        }

        public StockManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}