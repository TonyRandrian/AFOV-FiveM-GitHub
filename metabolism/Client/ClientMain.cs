using CitizenFX.Core;
using metabolism.Shared;
using System;
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
        }

        private void OnStateUpdated(string gaugesJson)
        {
            SendNuiMessage("{\"action\":\"updateState\", \"value\":" + gaugesJson + "}");
        }

        [Tick]
        public Task OnHudTick()
        {
            bool shouldShow = !(IsPauseMenuActive() 
                || IsWarningMessageActive() 
                || IsCutsceneActive() 
                || GetIsLoadingScreenActive());

            Debug.WriteLine($"Should show : {shouldShow}");
            Debug.WriteLine($"Should show & _isAllHudVisible: {shouldShow && _isAllHudVisible}");

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
