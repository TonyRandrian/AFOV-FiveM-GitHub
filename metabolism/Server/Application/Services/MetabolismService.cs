using CitizenFX.Core;
using metabolism.Server.Application.Abstractions;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using metabolism.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace metabolism.Server.Application.Services
{
    public class MetabolismService
    {
        private readonly IReadOnlyList<GaugeDefinition> _definitions;
        private readonly PlayerRegistry _registry;
        private readonly IClientNotifier _notifier;


        public MetabolismService(
            IGaugeConfigProvider configProvider,
            PlayerRegistry registry,
            IClientNotifier notifier)
        {
            _definitions = configProvider.LoadGauges();
            _registry = registry;
            _notifier = notifier;
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

            List<CharacterGauge> gauges = _definitions
                .Select(definition => new CharacterGauge(definition))
                .ToList();

            _registry.Add(new PlayerState(player, character, gauges));
            Debug.WriteLine($"[Metabolism] Player registered : {player.Name}");
        }

        public void UnregisterPlayer(string handle)
        {
            if (_registry.TryGet(handle, out PlayerState state))
            {
                state.Character.IsOnline = false;

                _registry.Remove(handle);
                Debug.WriteLine($"[Metabolism] Player unregistered : {state.Character.Nickname}");
            }
        }

        public void SetActivity(string handle, PlayerActivity activity)
        {
            if (_registry.TryGet(handle, out PlayerState state))
            {
                state.Activity = activity;
            }
        }

        public void Update(float deltaSeconds)
        {
            foreach (PlayerState state in _registry.Snapshot())
            {
                try
                {
                    UpdateGauges(state, deltaSeconds);
                    _notifier.SendGauges(state);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Metabolism] Update failed for {state.Handle} : {ex}");
                }
            }
        }

        private static void UpdateGauges(PlayerState state, float deltaSeconds)
        {
            foreach (CharacterGauge gauge in state.Gauges)
            {
                GaugeVariationCondition condition = gauge.Definition.VariationCondition;

                if (condition == GaugeVariationCondition.CUSTOM)
                {
                    continue;
                }

                if (IsActive(condition, state.Activity))
                {
                    gauge.Drain(deltaSeconds);
                }
                else
                {
                    gauge.Regen(deltaSeconds);
                }
            }
        }

        private static bool IsActive(GaugeVariationCondition condition, PlayerActivity activity)
        {
            switch (condition)
            {
                case GaugeVariationCondition.ALWAYS:
                    return true;
                case GaugeVariationCondition.UNDERWATER:
                    return activity.HasFlag(PlayerActivity.Underwater);
                case GaugeVariationCondition.SPRINTING:
                    return activity.HasFlag(PlayerActivity.Sprinting);
                default:
                    return false;
            }
        }
    }
}
