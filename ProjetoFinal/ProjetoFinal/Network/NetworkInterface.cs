using System;

using Lidgren.Network;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Network
{
    public interface NetworkInterface : IDisposable
    {
        void Connect();

        void Disconnect();

        NetIncomingMessage ReadMessage();

        void Recycle(NetIncomingMessage im);

        void SendMessage(IGameMessage gameMessage);

        void SendMessageToClient(short id, IGameMessage gameMessage);

        NetOutgoingMessage CreateMessage();

        void RegisterClient(short clientCounter, NetConnection senderConnection);
    }
}