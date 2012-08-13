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
                om.Write((byte)gameMessage.MessageType);
                gameMessage.Encode(om);

                netServer.SendToAll(om, NetDeliveryMethod.ReliableUnordered);
            }
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
