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
        public CharacterHealthStatus Health { get; }
        public IReadOnlyList<CharacterGaugeStatus> Gauges { get; }
        public IReadOnlyList<BodyPartCharacterStatus> BodyPartsStatus { get; }
        public PlayerActivity Activity { get; set; } = PlayerActivity.None;


        public PlayerState(
            Player player, 
            Character character, 
            IReadOnlyList<CharacterGaugeStatus> gauges, 
            IReadOnlyList<BodyPartCharacterStatus> bodyParts)
        {
            Player = player;
            Handle = player.Handle;
            Character = character;
            Health = new CharacterHealthStatus(100f);
            Gauges = gauges;
            BodyPartsStatus = bodyParts;
        }
    }
}
