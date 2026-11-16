using CitizenFX.Core;
using metabolism.Server.Application.Services;
using metabolism.Shared;

namespace metabolism.Server.Application.Handlers
{
    public class PlayerEventsHandler
    {
        private readonly PlayerSessionService _sessions;
        private readonly GaugeService _gauges;
        private readonly BodyPartService _bodyParts;


        public PlayerEventsHandler(
            PlayerSessionService sessions,
            GaugeService gauges,
            BodyPartService bodyParts
            )
        {
            _sessions = sessions;
            _gauges = gauges;
            _bodyParts = bodyParts;
        }

        public void OnJoined([FromSource] Player player)
        {
            _sessions.RegisterPlayer(player);
        }

        public void OnDropped([FromSource] Player player)
        {
            _sessions.UnregisterPlayer(player.Handle);
        }

        public void OnActivityChanged([FromSource] Player player, int activity)
        {
            int allowed = (int)(PlayerActivity.Underwater | PlayerActivity.Sprinting);
            _gauges.SetActivity(player.Handle, (PlayerActivity)(activity & allowed));
        }

        public void OnPlayerDamaged([FromSource] Player player, int boneId, int damageAmount, DamageSource source)
        {
            _bodyParts.ApplyBoneDamage(player.Handle, boneId, damageAmount, source); 
        }
    }
}
