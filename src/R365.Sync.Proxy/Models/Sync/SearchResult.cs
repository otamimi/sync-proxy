using System.Collections.Generic;

namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// Indicates the result of a search, including a total count and the list of entities that matched
/// the search.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="Entities">
/// The entities matching the search parameters, within any specified paging limit.
/// </param>
/// <param name="TotalCount">
/// The total count for all entities that matched the search parameters.
/// </param>
public record SearchResult<TEntity>(IEnumerable<TEntity> Entities, int TotalCount);
