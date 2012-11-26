using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class ClientDisconnectedMessage : PlayerIdMessage
    {
        public override GameMessageType GameMessageType { get { return GameMessageType.ClientDisconnected; } }

        public ClientDisconnectedMessage(NetIncomingMessage im) : base(im)
        {

        }

        public ClientDisconnectedMessage(short id) : base(id)
        {

        }
    }
}
