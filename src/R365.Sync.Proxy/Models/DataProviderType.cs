namespace R365.Sync.Proxy.Models
{
    public enum DataProviderType : byte
    {
        Accounting = 1,
        HR = 2,
        Payroll = 3,
        Hire = 4,

        /// <summary>
        /// A data provider for syncing data with Radar, another R365 application.
        /// </summary>
        RadarInternal = 5,
    }
}