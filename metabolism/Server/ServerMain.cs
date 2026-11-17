using CitizenFX.Core;
using metabolism.Server.Application.Handlers;
using metabolism.Server.Application.Mappers;
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
        private readonly GaugeService _gauges;


        public ServerMain()
        {
            string resourceName = GetCurrentResourceName();

            JsonGaugeConfigProvider gaugeConfig = new JsonGaugeConfigProvider(resourceName);
            JsonBodyPartConfigProvider bodyPartConfig = new JsonBodyPartConfigProvider(resourceName);
            JsonDamageSourceConfigProvider damageSourceConfig = new JsonDamageSourceConfigProvider(resourceName);
            PlayerRegistry registry = new PlayerRegistry();
            ClientNotifier notifier = new ClientNotifier();
            BoneBodyPartMapper boneMapper = new BoneBodyPartMapper();

            PlayerSessionService sessions = new PlayerSessionService(gaugeConfig, bodyPartConfig, registry);
            _gauges = new GaugeService(registry, notifier);
            BodyPartService bodyParts = new BodyPartService(registry, notifier, boneMapper, damageSourceConfig);

            PlayerEventsHandler handler = new PlayerEventsHandler(sessions, _gauges, bodyParts);

            EventHandlers[Events.PLAYER_JOINED] += new Action<Player>(handler.OnJoined);
            EventHandlers[Events.PLAYER_DROPPED] += new Action<Player>(handler.OnDropped);
            EventHandlers[Events.PLAYER_ACTIVITY_CHANGED] += new Action<Player, int>(handler.OnActivityChanged);
            EventHandlers[Events.PLAYER_DAMAGED] += new Action<Player, int, float, int>(handler.OnPlayerDamaged);

            Debug.WriteLine("[Metabolism] Script started");
        }

        [Tick]
        private async Task OnTick()
        {
            await Delay(1000);
            _gauges.Update(1f);
        }
    }
}
