using Lidgren.Network;

namespace ProjetoFinal.Network.Messages
{
    public enum GameMessageType
    {
        ClientInfo,
        UpdatePlayerState,
        UpdatePlayerMovementState,
        UpdatePlayerStateWithArrow,
        PlayerHit,
    }

    public interface IGameMessage
    {
        GameMessageType GameMessageType { get; }

        void Encode(NetOutgoingMessage om);

        void Decode(NetIncomingMessage im);
    }
}