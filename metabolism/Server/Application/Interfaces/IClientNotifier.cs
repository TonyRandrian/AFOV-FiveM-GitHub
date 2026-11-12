using metabolism.Server.Application.Utils;

namespace metabolism.Server.Application.Interfaces
{
    public interface IClientNotifier
    {
        void SendGauges(PlayerState state);
        void SendBodyParts(PlayerState state);
    }
}
