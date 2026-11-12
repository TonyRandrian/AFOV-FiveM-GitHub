using CitizenFX.Core;
using metabolism.Server.Application.Handlers;
using metabolism.Server.Application.Services;
using metabolism.Server.Application.Utils;
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
        private readonly BodyPartService _bodyPart;


        public ServerMain()
        {
            JsonGaugeConfigProvider config = new JsonGaugeConfigProvider(GetCurrentResourceName());
            JsonBodyPartConfigProvider bodyConfig = new JsonBodyPartConfigProvider(GetCurrentResourceName());
            PlayerRegistry registry = new PlayerRegistry();
            ClientNotifier notifier = new ClientNotifier();

            _metabolism = new MetabolismService(bodyConfig, config, registry, notifier);
            _bodyPart = new BodyPartService(registry, notifier);

            PlayerEventsHandler handler = new PlayerEventsHandler(_metabolism, _bodyPart);

            EventHandlers[Events.PLAYER_JOINED] += new Action<Player>(handler.OnJoined);
            EventHandlers[Events.PLAYER_DROPPED] += new Action<Player>(handler.OnDropped);
            EventHandlers[Events.PLAYER_ACTIVITY_CHANGED] += new Action<Player, int>(handler.OnActivityChanged);
            EventHandlers[Events.PLAYER_DAMAGED] += new Action<Player, int, int>(handler.OnPlayerDamaged);

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
