using System;
using System.Collections.Generic;
using System.Text;

namespace R365.Sync.Proxy.Models
{
    public class GeneralLedgerAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// GL Account sub-type. Maps to GlTypeType in R365 parlance
        /// </summary>
        public string SubType { get; set; }
        public ulong ConcurrencyToken { get; set; }

        internal const string RouteName = "accounts";
    }

    public class GeneralLedgerAccountCount
    {
        public int Count { get; set; }
    }
}
