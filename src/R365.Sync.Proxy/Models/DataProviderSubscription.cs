using System;
using System.Collections.Generic;
using R365.Caching;

namespace R365.Sync.Proxy.Models
{
	[CachePolicy(CacheType.LocalVolatile, 43200)]
    public class DataProviderSubscription
    {
        public long Id { get; set; }
        public int DataProviderId { get; set; }
        public DataProvider DataProvider { get; set; }
        public IDictionary<string, string> Properties { get; set; }
        public Guid ConnectionAdminUserId { get; set; }

        internal const string RouteName = "dataprovider/subscription";
    }
}
