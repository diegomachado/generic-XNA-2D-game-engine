using System;

using Lidgren.Network;

using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Network
{
    /// <summary>
    /// TODO: Atualizar Sumário
    /// </summary>
    public interface INetworkManager : IDisposable
    {
        void Connect();

        void Disconnect();

        NetIncomingMessage ReadMessage();

        void Recycle(NetIncomingMessage im);

        void SendMessage(IGameMessage gameMessage);

        NetOutgoingMessage CreateMessage();
    }
}