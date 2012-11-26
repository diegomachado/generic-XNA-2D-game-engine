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
        public short ClientId { get; set; }
        public Dictionary<short, Client> ClientsInfo { get; set; }

        public GameMessageType GameMessageType
        {
            get { return GameMessageType.ClientInfo; }
        }

        public HailMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public HailMessage(short id, Dictionary<short, Client> clients)
        {
            ClientId = id;
            ClientsInfo = clients;
        }

        public void Decode(NetIncomingMessage im)
        {
            ClientId = im.ReadInt16();
            short numClients = im.ReadInt16();

            if (ClientsInfo == null)
                ClientsInfo = new Dictionary<short, Client>();

            for (int i = 0; i < numClients; i++)
                ClientsInfo.Add(im.ReadInt16(), new Client(im.ReadString()));
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(ClientId);
            om.Write((short)ClientsInfo.Count);
            
            foreach (KeyValuePair<short, Client> clientInfo in ClientsInfo)
            {
                om.Write(clientInfo.Key);
                om.Write(clientInfo.Value.nickname);
            }
        }
    }
}
