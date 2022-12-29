using System.Collections.Generic;

namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// The result of a single entity that was included in a batch sync.
/// </summary>
public class SyncHistoryEntity
{
    /// <summary>
    /// The type name of the entity that was synced.
    /// </summary>
    public string EntityTypeName { get; set; }

    /// <summary>
    /// The id of the entity that was synced.
    /// </summary>
    public string EntityId { get; set; }

    /// <summary>
    /// The sync status of the entity, indicating success or failure.
    /// </summary>
    public EntitySyncStatus SyncStatus { get; set; }

    /// <summary>
    /// If the sync failed, a list of errors indicating why the entity failed to sync.
    /// </summary>
    public List<SyncHistoryError> Errors { get; set; }
}
