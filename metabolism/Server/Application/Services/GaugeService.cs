using metabolism.Server.Application.Interfaces;
using metabolism.Server.Application.Utils;
using metabolism.Server.Domain;
using metabolism.Shared;
using System;
using System.Diagnostics;

namespace metabolism.Server.Application.Services
{
    public class GaugeService
    {
        private readonly PlayerRegistry _registry;
        private readonly IClientNotifier _notifier;


        public GaugeService(
            PlayerRegistry registry,
            IClientNotifier notifier)
        {
            _registry = registry;
            _notifier = notifier;
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

        public static void UpdateGauges(PlayerState state, float deltaSeconds)
        {
            foreach (CharacterGaugeStatus gauge in state.Gauges)
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
