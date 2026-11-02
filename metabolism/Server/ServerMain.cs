using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server
{
    public class ServerMain : BaseScript
    {
        private readonly Dictionary<string, float> _hunger = new Dictionary<string, float>();
        private readonly MetabolismConfig _config;


        public ServerMain()
        {
            Debug.WriteLine("Metabolism script started!");
            string json = LoadResourceFile(GetCurrentResourceName(), "config.json");

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.WriteLine("[Metabolism] Unable to read config.json");
                return;
            }

            _config = JsonConvert.DeserializeObject<MetabolismConfig>(json);

            if (_config?.Hunger == null)
            {
                Debug.WriteLine("[Metabolism] Hunger configuration invalid");
                return;
            }

            Debug.WriteLine($"[Metabolism] Drain : {_config.Hunger.Drain}");

            _ = Run();
        }

        private async Task Run()
        {
            while (true)
            {
                await Delay(1000);

                foreach (Player player in Players)
                {
                    string playerId = player.Handle;

                    if (!_hunger.ContainsKey(playerId))
                    {
                        _hunger[playerId] = _config.Hunger.Max;
                    }

                    _hunger[playerId] = System.Math.Max(0.0f, _hunger[playerId] - _config.Hunger.Drain);

                    TriggerClientEvent(
                        player,
                        "metabolism:updateHunger",
                        _hunger[playerId]
                        );
                }
            }
        }
    }

    public class MetabolismConfig
    {
        [JsonProperty("hunger")]
        public HungerConfig Hunger { get; set; }
    }

    public class HungerConfig
    {
        [JsonProperty("max")]
        public float Max { get; set; }

        [JsonProperty("drain")]
        public float Drain { get; set; }
    }
}
