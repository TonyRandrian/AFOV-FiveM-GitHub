using Newtonsoft.Json;

namespace metabolism.Server.Models.Gauge
{
    public class Gauge
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("max")]
        public float Max { get; set; }

        [JsonProperty("regen")]
        public float Regen { get; set; }

        [JsonProperty("drain")]
        public float Drain { get; set; }
    }
}
