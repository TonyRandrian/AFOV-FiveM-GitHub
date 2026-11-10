using CitizenFX.Core;
using metabolism.Server.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server
{
    public class ServerMain : BaseScript
    {
        private List<Player> _connectedPlayers;
        private readonly List<Gauge> _gauges;

        public ServerMain()
        {
            _connectedPlayers = new List<Player>();
            _gauges = LoadGauges();

            Debug.WriteLine("Metabolism script started!");

            EventHandlers[Events.PLAYER_JOINED] +=
                new Action<Player>(OnPlayerJoined);

            EventHandlers[Events.PLAYER_DROPPED] +=
                new Action<Player>(OnPlayerDropped);
        }

        private static List<Gauge> LoadGauges()
        {
            string configJson = LoadResourceFile(GetCurrentResourceName(), "config.json");

            if (string.IsNullOrWhiteSpace(configJson))
            {
                throw new InvalidOperationException(
                    "[Metabolism] Unable to load config.json.");
            }

            List<Gauge> gauges = JsonConvert.DeserializeObject<List<Gauge>>(configJson) ?? throw new InvalidOperationException(
                    "[Metabolism] config.json content not valid.");

            Debug.WriteLine($"[Metabolism] {gauges.Count} gauges loaded.");
            return gauges;
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
    }
}
