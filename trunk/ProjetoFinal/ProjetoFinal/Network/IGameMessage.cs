using Lidgren.Network;

namespace ProjetoFinal.Network.Messages
{
    public enum GameMessageType
    {
        PlayerId,
        ClientDisconnected,
        ClientInfo,
        UpdatePlayerState,
        UpdatePlayerMovementState,
        UpdatePlayerStateWithArrow,
        PlayerHit,
        PlayerCreated,
        PlayerSpawned,
        NewClientPlayerCreated
    }

    public interface IGameMessage
    {
        GameMessageType GameMessageType { get; }

        void Encode(NetOutgoingMessage om);

        void Decode(NetIncomingMessage im);
    }
}