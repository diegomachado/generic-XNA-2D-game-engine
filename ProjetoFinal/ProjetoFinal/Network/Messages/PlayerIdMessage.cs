using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerIdMessage : IGameMessage
    {
        public short PlayerId { get; set; }

        public virtual GameMessageType GameMessageType { get { return GameMessageType.PlayerId; } }

        public PlayerIdMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public PlayerIdMessage(short id)
        {
            PlayerId = id;
        }

        public virtual void Decode(NetIncomingMessage im)
        {
            PlayerId = im.ReadInt16();
        }

        public virtual void Encode(NetOutgoingMessage om)
        {
            om.Write(PlayerId);
        }
    }
}
