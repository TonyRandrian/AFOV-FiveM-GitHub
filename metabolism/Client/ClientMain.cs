using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class ClientMain : BaseScript
    {
        private bool isMenuVisible = false;

        public ClientMain()
        {
            EventHandlers[Events.UPDATE_STATE] +=
                new Action<string>(OnStateUpdated);
        }

        private void OnStateUpdated(string gaugesJson)
        {
            string json = "{\"action\":\"updateState\", \"value\":" + gaugesJson + "}";
            Debug.WriteLine(gaugesJson);
            SendNuiMessage(json);
        }

        [Tick]
        public Task OnTick()
        {
            if (IsControlJustPressed(0, 311))
            {
                ToggleMenu(!isMenuVisible);
            }

            return Task.FromResult(0);
        }

        private void ToggleMenu(bool visible)
        {
            string json = "{\"action\":\"setVisible\",\"visible\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
            isMenuVisible = visible;
        }
    }
}
