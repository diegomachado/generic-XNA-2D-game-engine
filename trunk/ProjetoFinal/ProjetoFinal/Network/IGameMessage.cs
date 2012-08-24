﻿using Lidgren.Network;

namespace ProjetoFinal.Network.Messages
{
    public enum GameMessageType
    {
        ClientInfo,
        UpdatePlayerState
    }

    public interface IGameMessage
    {
        GameMessageType MessageType { get; }

        void Encode(NetOutgoingMessage om);

        void Decode(NetIncomingMessage im);
    }
}