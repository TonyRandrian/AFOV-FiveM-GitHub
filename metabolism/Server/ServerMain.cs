using CitizenFX.Core;
using metabolism.Server.Application;
using metabolism.Server.Handlers;
using metabolism.Server.Infrastructure.Config;
using metabolism.Server.Infrastructure.Network;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server
{
    public class ServerMain : BaseScript
    {
        private readonly MetabolismService _metabolism;


        public ServerMain()
        {
            JsonGaugeConfigProvider config = new JsonGaugeConfigProvider(GetCurrentResourceName());
            PlayerRegistry registry = new PlayerRegistry();
            ClientNotifier notifier = new ClientNotifier();

            _metabolism = new MetabolismService(config, registry, notifier);
            PlayerEventsHandler handler = new PlayerEventsHandler(_metabolism);

            EventHandlers[Events.PLAYER_JOINED] += new Action<Player>(handler.OnJoined);
            EventHandlers[Events.PLAYER_DROPPED] += new Action<Player>(handler.OnDropped);
            EventHandlers[Events.PLAYER_ACTIVITY_CHANGED] += new Action<Player, int>(handler.OnActivityChanged);

            Debug.WriteLine("[Metabolism] Script started");
        }

        [Tick]
        private async Task OnTick()
        {
            await Delay(1000);
            _metabolism.Update(1f);
        }
    }
}
