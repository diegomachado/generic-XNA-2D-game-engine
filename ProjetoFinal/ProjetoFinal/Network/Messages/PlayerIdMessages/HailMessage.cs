using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class HailMessage : PlayerIdMessage
    {
        public Dictionary<short, Client> ClientsInfo { get; set; }

        public override GameMessageType GameMessageType { get { return GameMessageType.ClientInfo; } }

        public HailMessage(NetIncomingMessage im) : base(im)
        {

        }

        public HailMessage(short id, Dictionary<short, Client> clients) : base(id)
        {
            ClientsInfo = clients;
        }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            short numClients = im.ReadInt16();

            if (ClientsInfo == null)
                ClientsInfo = new Dictionary<short, Client>();

            for (int i = 0; i < numClients; i++)
                ClientsInfo.Add(im.ReadInt16(), new Client(im.ReadString()));
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write((short)ClientsInfo.Count);
            
            foreach (KeyValuePair<short, Client> clientInfo in ClientsInfo)
            {
                om.Write(clientInfo.Key);
                om.Write(clientInfo.Value.nickname);
            }
        }
    }
}
