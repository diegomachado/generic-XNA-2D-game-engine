using System;
using System.Collections.Generic;

using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class ClientConnectedEventArgs : EventArgs
    {
        public short clientId { get; set; }
        public Dictionary<short, Client> clientsInfo { get; set; }

        public ClientConnectedEventArgs(HailMessage hailMessage)
        {
            this.clientId = hailMessage.clientId;
            this.clientsInfo = hailMessage.clientsInfo;
        }
    }
}