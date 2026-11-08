using CitizenFX.Core;
using metabolism.Server.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using System;

namespace metabolism.Server
{
    public class ServerMain : BaseScript
    {
        private readonly Dictionary<string, float> _gauge = new Dictionary<string, float>();
        private readonly Metabolism _config;


        public ServerMain()
        {
            Debug.WriteLine("Metabolism script started!");
            string json = LoadResourceFile(GetCurrentResourceName(), "config.json");

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.WriteLine("[Metabolism] Unable to read config.json");
                return;
            }
            
            _config = JsonConvert.DeserializeObject<Metabolism>(json);

            //_ = Run();

            EventHandlers["playerJoining"] +=
                new Action<Player>(OnPlayerJoined);
        }

        private void OnPlayerJoined([FromSource] Player player)
        {
            Debug.WriteLine($"[Metabolism] Player joined : {player.Name}");
        }

        private async Task Run()
        {
            while (true)
            {
                await Delay(1000);

                foreach (Player player in Players)
                {
                    string playerId = player.Handle;

                    if (!_gauge.ContainsKey(playerId))
                    {
                        _gauge[playerId] = _config.Hunger.Max;
                    }

                    _gauge[playerId] = System.Math.Max(0.0f, _gauge[playerId] - _config.Hunger.Drain);

                    TriggerClientEvent(
                        player,
                        "metabolism:updateHunger",
                        _gauge[playerId]
                        );
                }
            }
        }
    }
}
