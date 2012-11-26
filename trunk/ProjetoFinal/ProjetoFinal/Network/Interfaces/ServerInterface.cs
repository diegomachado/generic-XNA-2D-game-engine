using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Network
{
    public class ServerInterface : NetworkInterface
    {
        public int port {get; set;}

        private NetServer netServer;
        private Dictionary<short, long> clientId_uid = new Dictionary<short,long>();
        private bool isDisposed;

        public void Connect()
        {
            var config = new NetPeerConfiguration("ProjetoFinal")
            {
                Port = port,
                //SimulatedMinimumLatency = 0.2f, 
                //SimulatedLoss = 0.1f 
            };

            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            netServer = new NetServer(config);
            netServer.Start();
        }

        public void Disconnect()
        {
            netServer.Shutdown("Bye");
        }

        public NetIncomingMessage ReadMessage()
        {
            return this.netServer.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            this.netServer.Recycle(im);
        }

        public void SendMessage(IGameMessage gameMessage)
        {
            if (this.netServer.ConnectionsCount > 0)
            {
                NetOutgoingMessage om = netServer.CreateMessage();
                om.Write((byte)gameMessage.GameMessageType);
                gameMessage.Encode(om);

                netServer.SendToAll(om, NetDeliveryMethod.ReliableUnordered);
            }
        }

        public void SendMessageToClient(short id, IGameMessage gameMessage)
        {
            if (this.netServer.ConnectionsCount > 0)
            {
                NetOutgoingMessage om = netServer.CreateMessage();
                om.Write((byte)gameMessage.GameMessageType);
                gameMessage.Encode(om);

                netServer.SendMessage(om, netServer.Connections.Find(item => item.RemoteUniqueIdentifier == clientId_uid[id]), NetDeliveryMethod.ReliableUnordered);
            }
        }

        public void RegisterClient(short id, NetConnection connection)
        {
            clientId_uid.Add(id, connection.RemoteUniqueIdentifier);
        }

        public short GetPlayerIdFromConnection(NetConnection netConnection)
        {
            return clientId_uid.First(x => x.Value == netConnection.RemoteUniqueIdentifier).Key;
        }

        public NetOutgoingMessage CreateMessage()
        {
            return this.netServer.CreateMessage();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                    this.Disconnect();
                
                this.isDisposed = true;
            }
        }
    }
}
