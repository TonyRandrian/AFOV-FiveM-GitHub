using CitizenFX.Core;
using metabolism.Server.Domain;
using metabolism.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace metabolism.Server.Application
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
