using CitizenFX.Core;
using metabolism.Server.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using System;
using System.Linq;

namespace metabolism.Server
{
    public class ServerMain : BaseScript
    {
        private readonly Dictionary<string, float> _gauge = new Dictionary<string, float>();
        private readonly Metabolism _config;
        private readonly List<Player> _connectedPlayers = new List<Player>();


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

            _ = Run();

            EventHandlers["playerJoining"] +=
                new Action<Player>(OnPlayerJoined);

            EventHandlers["playerDropped"] +=
                new Action<Player>(OnPlayerDropped);

            
        }

        private void OnPlayerJoined([FromSource] Player player)
        {
            Debug.WriteLine($"[Metabolism] Player joined : {player.Name}");
            _connectedPlayers.Add(player);
        }

        private void OnPlayerDropped([FromSource] Player player)
        {
            Debug.WriteLine($"[Metabolism] Player dropped : {player.Name}");
            _connectedPlayers.Remove(player);
        }

        private async Task Run()
        {
            while (true)
            {
                await Delay(1000);

                foreach (Player player in _connectedPlayers.ToList())
                {
                    string playerId = player.Handle;

                    if (!_gauge.ContainsKey(playerId))
                    {
                        
                    }

                    
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
