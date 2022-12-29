namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// Indicates the current sync status for this batch.
/// </summary>
public enum BatchSyncStatus : byte
{
    /// <summary>
    /// The batch has not started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The batch is currently being processed.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The batch has been completely processed. There may be a mix of successfully synced and
    /// failed entities.
    /// </summary>
    Finished = 2,

    /// <summary>
    /// The batch sync failed to complete due to some fatal error. Entities may have been partially
    /// synced, depending on when the failure occurred.
    /// </summary>
    Failed = 3,
}
