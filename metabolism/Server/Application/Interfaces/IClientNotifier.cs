using metabolism.Server.Application.Utils;
using metabolism.Shared;

namespace metabolism.Server.Application.Interfaces
{
    public interface IClientNotifier
    {
        void SendGauges(PlayerState state);
        void SendBodyParts(PlayerState state);
        void SendPlayerDeath(PlayerState state);
        void SendRestrictions(PlayerState state, ActionRestriction restrictions);
        void SendMovementClipset(PlayerState state, string clipset);
    }
}
