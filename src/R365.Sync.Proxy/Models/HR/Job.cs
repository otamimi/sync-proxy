using R365.Caching;

namespace R365.Sync.Proxy.Models
{
    public class Job
    {
        public string CountryCode { get; set; }
        public string JobCode { get; set; }
        public string Title { get; set; }
        public string JobFamilyCode { get; set; }
        public bool IsActive { get; set; }
    }
}