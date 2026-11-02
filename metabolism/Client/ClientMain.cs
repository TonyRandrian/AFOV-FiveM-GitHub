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
            EventHandlers["metabolism:updateHunger"] +=
                new Action<float>(OnHungerUpdated);
        }

        private void OnHungerUpdated(float hunger)
        {
            string json = "{\"action\":\"gaugeVariation\", \"value\":" + hunger + "}";
            SendNuiMessage(json);
        }

        [Tick]
        public Task OnTick()
        {
            if (IsControlJustPressed(0, 311))
            {
                Debug.WriteLine("Touch K pressed");
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
