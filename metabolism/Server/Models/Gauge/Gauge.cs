using Newtonsoft.Json;

namespace metabolism.Server.Models.Gauge
{
    public class Gauge
    {
        [JsonProperty("max")]
        public float Max { get; set; }

        [JsonProperty("regen")]
        public float Regen { get; set; }

        [JsonProperty("drain")]
        public float Drain { get; set; }
    }
}
