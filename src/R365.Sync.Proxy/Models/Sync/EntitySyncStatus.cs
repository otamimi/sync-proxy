namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// The sync status of a specific entity that was part of a sync batch.
/// </summary>
public enum EntitySyncStatus : byte
{
    /// <summary>
    /// Indicates the entity failed to sync.
    /// </summary>
    Failed = 0,

    /// <summary>
    /// Indicates the entity was synced successfully.
    /// </summary>
    Synced = 1,
}
