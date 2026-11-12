using CitizenFX.Core;
using metabolism.Server.Application.Services;
using metabolism.Shared;

namespace metabolism.Server.Application.Handlers
{
    public class PlayerEventsHandler
    {
        private readonly MetabolismService _metabolism;
        private readonly BoneService _bone;


        public PlayerEventsHandler(MetabolismService metabolism, BoneService bone)
        {
            _metabolism = metabolism;
            _bone = bone;
        }

        public void OnJoined([FromSource] Player player)
        {
            _metabolism.RegisterPlayer(player);
        }

        public void OnDropped([FromSource] Player player)
        {
            _metabolism.UnregisterPlayer(player.Handle);
        }

        public void OnActivityChanged([FromSource] Player player, int activity)
        {
            int allowed = (int)(PlayerActivity.Underwater | PlayerActivity.Sprinting);
            _metabolism.SetActivity(player.Handle, (PlayerActivity)(activity & allowed));
        }

        public void OnPlayerDamaged([FromSource] Player player, int boneId)
        {
            CitizenFX.Core.Debug.WriteLine($"{player.Name} : {_bone.ToBodyPart(boneId)}");
        }
    }
}
