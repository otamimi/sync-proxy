using System.Text.Json;
using System.Text.Json.Serialization;

namespace R365.Sync.Proxy.Models
{
    public class ChangeEvent
    {
        [JsonPropertyName("eventType")]
        public EventType EventType { get; set; }

        [JsonPropertyName("entityTypeName")]
        public string EntityTypeName { get; set; }

        [JsonPropertyName("entity")]
        public object Entity { get; set; }

        internal const string RouteName = "event";
    }
}
