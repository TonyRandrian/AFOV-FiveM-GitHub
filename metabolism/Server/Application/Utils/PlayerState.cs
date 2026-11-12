using CitizenFX.Core;
using metabolism.Server.Domain;
using metabolism.Shared;
using System.Collections.Generic;

namespace metabolism.Server.Application.Utils
{
    public class PlayerState
    {
        public string Handle { get; }
        public Player Player { get; }
        public Character Character { get; }
        public IReadOnlyList<CharacterGauge> Gauges { get; }
        public PlayerActivity Activity { get; set; } = PlayerActivity.None;


        public PlayerState(Player player, Character character, IReadOnlyList<CharacterGauge> gauges)
        {
            Player = player;
            Handle = player.Handle;
            Character = character;
            Gauges = gauges;
        }
    }
}
