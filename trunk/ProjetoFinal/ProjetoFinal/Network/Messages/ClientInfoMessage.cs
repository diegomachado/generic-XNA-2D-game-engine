using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class ClientInfoMessage : IGameMessage
    {
        Dictionary<String, String> clientsInfo;

        public GameMessageTypes MessageType
        {
            get { return GameMessageTypes.ClientInfo; }
        }

        public void Decode(NetIncomingMessage im)
        {
            short numClients = im.ReadInt16();

            for (int i = 0; i < numClients; i++)
                clientsInfo.Add(im.ReadString(), im.ReadString());
        }

        public void Encode(NetOutgoingMessage om)
        {
            short numClients = (short)clientsInfo.Count;
            
            om.Write(numClients);

            foreach (KeyValuePair<string, string> clientInfo in clientsInfo)
            {
                om.Write(clientInfo.Key);
                om.Write(clientInfo.Value);
            }

            
                

            
            
        }
    }
}
