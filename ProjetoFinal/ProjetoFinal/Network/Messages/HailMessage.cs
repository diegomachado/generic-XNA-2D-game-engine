using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class HailMessage : IGameMessage
    {
        public short clientId { get; set; }
        public Dictionary<short, Client> clientsInfo { get; set; }

        public GameMessageTypes MessageType
        {
            get { return GameMessageTypes.ClientInfo; }
        }

        public HailMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public HailMessage(short id, Dictionary<short, Client> clients)
        {
            clientId = id;
            clientsInfo = clients;
        }

        public void Decode(NetIncomingMessage im)
        {
            short clientId = im.ReadInt16();
            short numClients = im.ReadInt16();

            if (clientsInfo == null)
                clientsInfo = new Dictionary<short, Client>();

            for (int i = 0; i < numClients; i++)
                clientsInfo.Add(im.ReadInt16(), new Client(im.ReadString()));
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(clientId);
            om.Write((short)clientsInfo.Count);
            //short numClients = (short)clientsInfo.Count;
            
            foreach (KeyValuePair<short, Client> clientInfo in clientsInfo)
            {
                om.Write(clientInfo.Key);
                om.Write(clientInfo.Value.nickname);
            }
        }
    }
}
