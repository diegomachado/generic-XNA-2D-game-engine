﻿using Lidgren.Network;

namespace ProjetoFinal.Network.Messages
{
    public enum GameMessageTypes
    {
        ClientInfo,
        UpdatePlayerState
    }

    public interface IGameMessage
    {
        GameMessageTypes MessageType { get; }

        void Encode(NetOutgoingMessage om);

        void Decode(NetIncomingMessage im);
    }
}