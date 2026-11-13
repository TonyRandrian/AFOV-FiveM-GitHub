using CitizenFX.Core;
using metabolism.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class ClientMain : BaseScript
    {
        private bool _isHudVisible = false;
        private bool _isAllHudVisible = true;
        private PlayerActivity _lastActivity = PlayerActivity.None;


        public ClientMain()
        {
            EventHandlers[Events.UPDATE_STATE] +=
                new Action<string>(OnStateUpdated);
            EventHandlers[Events.GAME_EVENT_TRIGGERED] +=
                new Action<string, List<object>>(OnGameEventTriggered);
            EventHandlers[Events.UPDATE_BODY_PART] += 
                new Action<string>(OnBodyPartUpdated);
        }

        private void OnBodyPartUpdated(string bodyPartJson)
        {
            SendNuiMessage("{\"action\":\"updateBodyParts\", \"value\":" + bodyPartJson + "}");
        }

        private void OnStateUpdated(string gaugesJson)
        {
            SendNuiMessage("{\"action\":\"updateState\", \"value\":" + gaugesJson + "}");
        }

        private void OnGameEventTriggered(string eventName, List<object> args)
        {
            if (eventName == "CEventNetworkEntityDamage")
            {
                int victimHandle = Convert.ToInt32(args[0]);

                if (DoesEntityExist(victimHandle) && IsEntityAPed(victimHandle))
                {
                    if (victimHandle == Game.PlayerPed.Handle)
                    {
                        int boneId = 0;
                        bool success = GetPedLastDamageBone(victimHandle, ref boneId);

                        int damageRaw = Convert.ToInt32(args[2]);
                        float damageAmount = BitConverter.ToSingle(BitConverter.GetBytes(damageRaw), 0);

                        if (success && boneId != 0)
                        {
                            TriggerServerEvent(Events.PLAYER_DAMAGED, boneId, damageAmount);
                        }
                    }
                }
            }
        }

        [Tick]
        public Task OnHudTick()
        {
            bool shouldShow = !(IsPauseMenuActive() 
                || IsWarningMessageActive() 
                || IsCutsceneActive() 
                || GetIsLoadingScreenActive());

            if (shouldShow != _isAllHudVisible)
            {
                SetAllHudVisible(shouldShow);
            }
            
            if (IsControlJustPressed(0, 311))
            {
                SetHudVisible(!_isHudVisible);
            }

            return Task.FromResult(0);
        }

        [Tick]
        public async Task OnActivityTick()
        {
            await Delay(250);

            int ped = PlayerPedId();
            PlayerActivity current = PlayerActivity.None;

            if (IsPedSwimmingUnderWater(ped))
            {
                current |= PlayerActivity.Underwater;
            }
            if (IsPedSprinting(ped))
            {
                current |= PlayerActivity.Sprinting;
            }

            if (current != _lastActivity)
            {
                _lastActivity = current;
                TriggerServerEvent(Events.PLAYER_ACTIVITY_CHANGED, (int)current);
            }
        }

        private void SetHudVisible(bool visible)
        {
            _isHudVisible = visible;
            string json = "{\"action\":\"setHudVisible\",\"value\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
        }

        private void SetAllHudVisible(bool visible)
        {
            _isAllHudVisible = visible;
            string json = "{\"action\":\"setAllHudVisible\",\"value\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
        }
    }
}
