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
    enum UpdatePlayerStateType : short
    {
        Action,
        Horizontal,
        Vertical
    }

    class UpdatePlayerStateMessage : IGameMessage
    {
        public short PlayerId { get; set; }
        public double MessageTime { get; set; }
        public Vector2 Position { get; set; }
        public short PlayerState { get; set; }
        public UpdatePlayerStateType StateType { get; set; }
        
        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            // TODO: Confirmar se chama o Decode dos filhos antes
            Decode(im);
        }

        public UpdatePlayerStateMessage(short id, Vector2 pos, short state, UpdatePlayerStateType st)
        {
            PlayerId = id;
            MessageTime = NetTime.Now;
            Position = pos;
            StateType = st;
            PlayerState = state;
        }

        public virtual GameMessageType GameMessageType { get { return GameMessageType.UpdatePlayerState; } }

        public virtual void Decode(NetIncomingMessage im)
        {
            PlayerId = im.ReadInt16();
            MessageTime = im.ReadDouble();
            Position = im.ReadVector2();
            StateType = (UpdatePlayerStateType)im.ReadInt16();
            PlayerState = im.ReadInt16();
        }

        public virtual void Encode(NetOutgoingMessage om)
        {
            om.Write(PlayerId);
            om.Write(MessageTime);
            om.Write(Position);
            om.Write((short)StateType);
            om.Write(PlayerState);
        }
    }
}
