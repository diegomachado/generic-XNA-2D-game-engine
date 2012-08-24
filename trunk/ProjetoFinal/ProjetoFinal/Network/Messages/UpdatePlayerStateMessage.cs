using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using Lidgren.Network;
using Lidgren.Network.Xna;

namespace ProjetoFinal.Network.Messages
{
    enum UpdatePlayerStateMessageType : short
    {
        Action,
        Horizontal,
        Vertical
    }

    class UpdatePlayerStateMessage : IGameMessage
    {
        public short playerId { get; set; }
        public double messageTime { get; set; }
        public Vector2 position { get; set; }
        public UpdatePlayerStateMessageType messageType { get; set; }
        
        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            // TODO: Confirmar se chama o Decode dos filhos antes
            Decode(im);
        }

        public UpdatePlayerStateMessage(short id, Player player, UpdatePlayerStateMessageType mt)
        {
            playerId = id;
            messageTime = NetTime.Now;
            position = player.Position;
        }

        public GameMessageType MessageType
        {
            get { return GameMessageType.UpdatePlayerState; }
        }

        public virtual void Decode(NetIncomingMessage im)
        {
            playerId = im.ReadInt16();
            messageTime = im.ReadDouble();
            position = im.ReadVector2();
            messageType = (UpdatePlayerStateMessageType)im.ReadInt16();
        }

        public virtual void Encode(NetOutgoingMessage om)
        {
            om.Write(playerId);
            om.Write(messageTime);
            om.Write(position);
            om.Write((short)messageType);
        }
    }
}
