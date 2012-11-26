using System;
using System.Collections.Generic;

using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class ClientConnectedEventArgs : EventArgs
    {
        public short ClientId { get; set; }
        public Dictionary<short, Client> ClientsInfo { get; set; }

        public ClientConnectedEventArgs(HailMessage hailMessage)
        {
            this.ClientId = hailMessage.ClientId;
            this.ClientsInfo = hailMessage.ClientsInfo;
        }
    }
}