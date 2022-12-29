using System;

namespace R365.Sync.Proxy.Models
{
    public class Vendor
    {
        public string Id { get; set; }
        public string PrimaryEmailAddr { get; set; }
        public bool Vendor1099 { get; set; }
        public string DomainName { get; set; }
        public string GivenName { get; set; }
        public string DisplayName { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public string SyncToken { get; set; }
        public string PrintOnCheckName { get; set; }
        public string FamilyName { get; set; }
        public string PrimaryPhone { get; set; }
        public string AcctNum { get; set; }
        public string CompanyName { get; set; }
        public bool Active { get; set; }
        public double Balance { get; set; }

        internal const string RouteName = "vendors";
    }

    public class VendorCount
    {
        public int Count { get; set; }
    }

    public class BillingAddress
    {
        public string City { get; set; }
        public string Line1 { get; set; }
        public string PostalCode { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string CountrySubDivisionCode { get; set; }
        public string Id { get; set; }
    }
}
