using Lidgren.Network;

namespace ProjetoFinal.Network.Messages
{
    public enum GameMessageType
    {
        ClientInfo,
        UpdatePlayerState,
        UpdatePlayerMovementState
    }

    public interface IGameMessage
    {
        GameMessageType GameMessageType { get; }

        void Encode(NetOutgoingMessage om);

        void Decode(NetIncomingMessage im);
    }
}