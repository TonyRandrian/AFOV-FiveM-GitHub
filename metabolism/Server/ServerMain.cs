using CitizenFX.Core;
using metabolism.Server.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server
{
    public class PlayerState
    {
        public Player Player { get; set; }
        public Character Character { get; set; }
        public List<CharacterGauge> Gauges { get; set; }
    }

    public class ServerMain : BaseScript
    {
        private Dictionary<string, PlayerState> _connectedPlayers;
        private readonly List<Gauge> _gauges;

        public ServerMain()
        {
            _connectedPlayers = new Dictionary<string, PlayerState>();
            _gauges = LoadGauges();

            Debug.WriteLine("Metabolism script started!");

            EventHandlers[Events.PLAYER_JOINED] +=
                new Action<Player>(OnPlayerJoined);

            EventHandlers[Events.PLAYER_DROPPED] +=
                new Action<Player>(OnPlayerDropped);

            _ = Run();
        }

        private async Task Run()
        {
            while (true)
            {
                await Delay(1000);

                foreach (PlayerState playerState  in _connectedPlayers.Values)
                {
                    UpdateGauges(playerState);
                }
            }
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

        private void UpdateGauges(PlayerState playerState)
        {
            foreach (CharacterGauge characterGauge in playerState.Gauges)
            {
                if (characterGauge.Gauge.VariationCondition != GaugeVariationCondition.Always)
                {
                    continue;
                }

                characterGauge.ActualValue = Math.Max(
                    characterGauge.Gauge.DefaultMinValue,
                    characterGauge.ActualValue - characterGauge.VariationDrain);
            }
            
            playerState.Player.TriggerEvent(
                Events.UPDATE_STATE,
                JsonConvert.SerializeObject(playerState.Gauges)
                );
        }

        private void OnPlayerJoined([FromSource] Player player)
        {
            Debug.WriteLine($"[Metabolism] Player joined : {player.Name}");

            if (!_connectedPlayers.ContainsKey(player.Handle))
            {
                _connectedPlayers.Add(player.Handle, null);
                List<CharacterGauge> gauges = new List<CharacterGauge>();

                Character character = new Character
                {
                    Id = Guid.NewGuid(),
                    Identifiers = player.Identifiers,
                    IsOnline = true,
                    Nickname = player.Name
                };

                foreach (Gauge gauge in _gauges)
                {
                    if (gauge.VariationCondition != GaugeVariationCondition.Always)
                    {
                        continue;
                    }

                    CharacterGauge characterGauge = new CharacterGauge
                    {
                        Id = Guid.NewGuid(),
                        Character = character,
                        Gauge = gauge,
                        MaxValue = gauge.DefaultMaxValue,
                        ActualValue = gauge.DefaultMaxValue,
                        VariationRegen = gauge.DefaultVariationRegen,
                        VariationDrain = gauge.DefaultVariationDrain
                    };

                    gauges.Add(characterGauge);
                }

                PlayerState playerState = new PlayerState
                {
                    Player = player,
                    Gauges = gauges,
                    Character = character
                };

                _connectedPlayers[player.Handle] = playerState;
            }
            else
            {
                PlayerState playerState = _connectedPlayers[player.Handle];
                playerState.Character.IsOnline = true;
            }
        }

        private void OnPlayerDropped([FromSource] Player player)
        {
            Debug.WriteLine($"[Metabolism] Player dropped : {player.Name}");
            PlayerState playerState = _connectedPlayers[player.Handle];
            playerState.Character.IsOnline = false;
        }
    }
}
