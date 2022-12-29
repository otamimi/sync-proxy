using System.Collections.Generic;
using Newtonsoft.Json;

namespace R365.Sync.Proxy.Models.Sync;

/// <summary>
/// An error that resulted from performing the sync. This may be used for an error for the sync in
/// general, or an error specific to a particular entity.
/// </summary>
public class SyncHistoryError
{
    /// <summary>
    /// The error code for this error. This error code should be used to look up a user-friendly
    /// message.
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// A human-readable description of the error. This is intended to help with debugging and may
    /// not be user-friendly.
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    /// Additional properties that may provide more details about the error.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> Properties { get; set; }
}
