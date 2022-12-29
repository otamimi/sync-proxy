using R365.Caching;

namespace R365.Sync.Proxy.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayrollId { get; set; }
        public string HireDate { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}