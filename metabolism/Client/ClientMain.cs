using CitizenFX.Core;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class ClientMain : BaseScript
    {
        bool isMenuVisible = false;

        public ClientMain()
        {
            Debug.WriteLine("Metabolism script enabled");
        }

        private void SetMenuVisible(bool visible)
        {
            string json = "{\"action\":\"setVisible\",\"visible\":" + (visible ? "true" : "false") + "}";
            SendNuiMessage(json);
        }

        [Tick]
        public Task OnTick()
        {
            if (IsControlJustPressed(0, 311))
            {
                SetMenuVisible(!isMenuVisible);
                isMenuVisible = !isMenuVisible;
            }

            return Task.FromResult(0);
        }
    }
}