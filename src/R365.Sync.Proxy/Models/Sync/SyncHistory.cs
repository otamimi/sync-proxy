using System.Collections.Generic;
using System;

namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// A lightweight Sync History DTO for returning query results. It does not include the list of
/// entities that are part of the history.
/// </summary>
public class SyncHistory
{
    /// <summary>
    /// The id of the sync history record.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The id of the subscription that was synced.
    /// </summary>
    public long DataProviderSubscriptionId { get; set; }

    /// <summary>
    /// The ultimate status of the sync.
    /// </summary>
    public BatchSyncStatus Status { get; set; }

    /// <summary>
    /// The timestamp the sync was started at.
    /// </summary>
    public DateTime StartedOn { get; set; }

    /// <summary>
    /// The id of the user who started the sync.
    /// </summary>
    public Guid StartedBy { get; set; }

    /// <summary>
    /// The timestamp the sync finished at.
    /// </summary>
    public DateTime? FinishedOn { get; set; }

    /// <summary>
    /// Any custom parameters that were specified to the sync provider.
    /// </summary>
    public IDictionary<string, object> Parameters { get; set; }

    /// <summary>
    /// The total number of entities that were successfully synced.
    /// </summary>
    public int EntitySyncCount { get; set; }

    /// <summary>
    /// The total number of entities that failed to sync.
    /// </summary>
    public int EntityFailureCount { get; set; }

    /// <summary>
    /// An error that may have prevented this sync from completing, if any.
    /// </summary>
    public SyncHistoryError SyncError { get; set; }
}
