using R365.Caching;

namespace R365.Sync.Proxy.Models
{
    public class Location
    {
        public string CountryCode { get; set; }
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipOrPostalCode { get; set; }
        public bool IsActive { get; set; }
    }
}