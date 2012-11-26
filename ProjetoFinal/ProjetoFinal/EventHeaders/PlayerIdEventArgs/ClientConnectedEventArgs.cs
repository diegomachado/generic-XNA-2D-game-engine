using System;
using System.Collections.Generic;

using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class ClientConnectedEventArgs : PlayerIdEventArgs
    {
        public Dictionary<short, Client> ClientsInfo { get; set; }

        public ClientConnectedEventArgs(HailMessage hailMessage)
        {
            this.PlayerId = hailMessage.PlayerId;
            this.ClientsInfo = hailMessage.ClientsInfo;
        }
    }
}