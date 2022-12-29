using System.Collections.Generic;

namespace R365.Sync.Proxy.Models
{
    public class RecordMap
    {
        public long Id { get; set; }
        public long DataProviderSubscriptionId { get; set; }
        public string RecordTypeName { get; set; }
        public string PropertyName { get; set; }

        /// <summary>
        /// Map of source and destination values
        /// </summary>
        public IDictionary<string, string> PropertyMaps { get; set; }

        internal const string RouteName = "recordmap";
    }
}
