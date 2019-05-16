﻿using StockManager.Core.Constants;

namespace StockManager.Core
{
    public class ErrorData
    {
        public ErrorData(ErrorCodes code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public ErrorData(ErrorCodes code)
            : this(code, code.ToString())
        {
        }

        public ErrorCodes Code { get; }

        public string Message { get; }
    }
}