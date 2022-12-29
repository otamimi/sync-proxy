namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// An error indicating that there is an error with mapping values from one system to another.
/// </summary>
/// <remarks>
/// These errors typically happen when R365 is missing mapped entities (e.g. Vendors, GL Accounts,
/// Locations). These entities must be added in R365 before the sync can be completed.
/// </remarks>
/// <param name="ErrorCode">The error code for the mapping error.</param>
/// <param name="PropertyName">The name of the property that could not be mapped.</param>
/// <param name="MissingValue">The value for which no mapped entity could be found.</param>
public record SyncHistoryMappingError(string ErrorCode, string PropertyName, string? MissingValue);
