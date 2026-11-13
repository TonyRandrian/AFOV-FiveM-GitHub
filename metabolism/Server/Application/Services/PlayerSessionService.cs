using CitizenFX.Core;
using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Application.Services
{
    public class PlayerSessionService
    {
        private readonly IReadOnlyList<GaugeDefinition> _gaugeDefinitions;
        private readonly IReadOnlyList<BodyPartDefinition> _bodyPartDefinitions;
        private readonly PlayerRegistry _registry;


        public PlayerSessionService(
            IGaugeConfigProvider gaugeConfigProvider,
            IBodyPartConfigProvider bodyPartConfigProvider,
            PlayerRegistry registry)
        {
            _gaugeDefinitions = gaugeConfigProvider.LoadGauges();
            _bodyPartDefinitions = bodyPartConfigProvider.LoadBodyParts();
            _registry = registry;
        }

        public void RegisterPlayer(Player player)
        {
            Character character = new Character
            {
                License = player.Identifiers["license"],
                Identifiers = player.Identifiers.ToList(),
                Nickname = player.Name,
                IsOnline = true
            };

            List<CharacterGauge> gauges = _gaugeDefinitions
                .Select(definition => new CharacterGauge(definition))
                .ToList();

            List<BodyPartCharacterStatus> bodyParts = _bodyPartDefinitions
                .Select(definition => new BodyPartCharacterStatus(definition))
                .ToList();

            _registry.Add(new PlayerState(player, character, gauges, bodyParts));
            Debug.WriteLine($"[Metabolism] Player registered : {player.Name}");
        }

        public void UnregisterPlayer(string handle)
        {
            if (_registry.TryGet(handle, out PlayerState state))
            {
                state.Character.IsOnline = false;

                _registry.Remove(handle);
                Debug.WriteLine($"[Metabolsim] Player unregistered : {state.Character.Nickname}");
            }
        }
    }
}
