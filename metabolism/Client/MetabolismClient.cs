using CitizenFX.Core;
using System;
using static CitizenFX.Core.Native.API;

namespace metabolism.Client
{
    public class MetabolismClient : BaseScript
    {
        public MetabolismClient()
        {
            EventHandlers["metabolism:updateHunger"] +=
                new Action<float>(OnHungerUpdated);
        }

        private void OnHungerUpdated(float hunger)
        {
            Debug.WriteLine("Hunger: " + hunger);
            string json = "{\"action\":\"gaugeVariation\", \"value\":" + hunger + "}";
            SendNuiMessage(json);
        }
    }
}
