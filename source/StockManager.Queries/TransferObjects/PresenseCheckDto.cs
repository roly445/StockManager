using System;
using System.Collections.Generic;
using System.Text;

namespace StockManager.Queries.TransferObjects
{
    public sealed class PresenceCheckDto<TPresence>
    {
        public TPresence PresenceIdentifier { get; set; }
    }
}