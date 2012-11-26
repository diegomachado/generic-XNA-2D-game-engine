using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Lidgren.Network;

using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Network
{
    public class ClientInterface : NetworkInterface
    {
        public int port {get; set;}
        public String ip {get; set;}

        private bool isDisposed;

        private NetClient netClient;
        
        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.Disconnect();
                }

                this.isDisposed = true;
            }
        }

        public void Connect()
        {
            var config = new NetPeerConfiguration("ProjetoFinal")
            {
                //SimulatedMinimumLatency = 0.2f, 
                //SimulatedLoss = 0.1f
            };

            config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this.netClient = new NetClient(config);
            this.netClient.Start();
            this.netClient.Connect(new IPEndPoint(NetUtility.Resolve(ip), port));
        }

        public void Disconnect()
        {
            this.netClient.Disconnect("Bye");
        }

        public NetIncomingMessage ReadMessage()
        {
            return this.netClient.ReadMessage();
        }

        public void Recycle(NetIncomingMessage im)
        {
            this.netClient.Recycle(im);
        }

        public void SendMessage(IGameMessage gameMessage)
        {
            if (this.netClient.ConnectionStatus == NetConnectionStatus.Connected)
            {
                NetOutgoingMessage om = this.netClient.CreateMessage();
                om.Write((byte)gameMessage.GameMessageType);
                gameMessage.Encode(om);

                this.netClient.SendMessage(om, NetDeliveryMethod.ReliableUnordered);
            }
        }

        public void SendMessageToClient(short id, IGameMessage gameMessage)
        {

        }

        public void RegisterClient(short clientCounter, NetConnection senderConnection)
        {

        }

        public NetOutgoingMessage CreateMessage()
        {
            return this.netClient.CreateMessage();
        }
    }
}
