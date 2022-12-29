namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// A Sieve-compatible model for specifying filtering, sorting, and paging.
/// </summary>
public class SyncSieveModel
{
    public string? Filters { get; set; }

    public string? Sorts { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}
