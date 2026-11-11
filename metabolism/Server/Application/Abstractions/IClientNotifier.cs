namespace metabolism.Server.Application.Abstractions
{
    public interface IClientNotifier
    {
        void SendGauges(PlayerState state);
    }
}
