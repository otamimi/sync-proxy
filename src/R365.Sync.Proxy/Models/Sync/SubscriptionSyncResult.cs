using System;
using System.Collections.Generic;

namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// The result of syncing a subscription.
/// </summary>
/// <param name="Id">The id of the sync history representing this sync attempt.</param>
/// <param name="DataProviderSubscriptionId">The id of the subscription that was synced.</param>
/// <param name="Status">The ultimate status of the sync.</param>
/// <param name="StartedOn">The timestamp the sync was started at.</param>
/// <param name="StartedBy">The id of the user who started the sync.</param>
/// <param name="FinishedOn">The timestamp the sync finished at.</param>
/// <param name="Parameters">Any custom parameters that were specified to the sync provider.</param>
/// <param name="EntitySyncCount">The total number of entities that were successfully synced.</param>
/// <param name="EntityFailureCount">The total number of entities that failed to sync.</param>
/// <param name="SyncError">An error that resulted from performing the sync.</param>
/// <param name="MappingErrors">
/// A list of mapping errors that prevented entities from being synced. These errors typically
/// happen when R365 is missing mapped entities (e.g. Vendors, GL Accounts, Locations). These
/// entities must be added in R365 before the sync can be completed.
/// </param>
public record SubscriptionSyncResult(
    string Id,
    long DataProviderSubscriptionId,
    BatchSyncStatus Status,
    DateTime StartedOn,
    Guid StartedBy,
    DateTime? FinishedOn,
    object Parameters,
    int EntitySyncCount,
    int EntityFailureCount,
    SyncHistoryError SyncError,
    IEnumerable<SyncHistoryMappingError> MappingErrors);
