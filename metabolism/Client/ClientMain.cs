using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class ClientMain : BaseScript
    {
        private bool isHudVisible = false;

        public ClientMain()
        {
            EventHandlers[Events.UPDATE_STATE] +=
                new Action<string>(OnStateUpdated);
        }

        private void OnStateUpdated(string gaugesJson)
        {
            string json = "{\"action\":\"updateState\", \"value\":" + gaugesJson + "}";
            SendNuiMessage(json);
        }

        [Tick]
        public Task OnTick()
        {
            
            if (IsControlJustPressed(0, 311))
            {
                ToggleHud(!isHudVisible);
            }

            return Task.FromResult(0);
        }

        private void ToggleHud(bool visible)
        {
            string json = "{\"action\":\"setHudVisible\",\"value\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
            isHudVisible = visible;
        }
    }
}
