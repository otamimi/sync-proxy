namespace R365.Sync.Proxy.Models
{
    public class DataProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DataProviderType Type { get; set; }
        public string Description { get; set; }
        public DataProviderAuthenticationType AuthenticationType { get; set; }
        public DataProviderMetadata DataProviderMetadata { get; set; }

        internal const string RouteName = "dataprovider";
    }
}
