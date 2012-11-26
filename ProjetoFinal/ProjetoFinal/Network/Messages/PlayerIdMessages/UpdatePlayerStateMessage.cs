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

    class UpdatePlayerStateMessage : PlayerIdMessage
    {
        public double MessageTime { get; set; }
        public Vector2 Position { get; set; }
        public short PlayerState { get; set; }
        public UpdatePlayerStateType StateType { get; set; }
        
        public UpdatePlayerStateMessage(NetIncomingMessage im) : base(im)
        {

        }

        public UpdatePlayerStateMessage(short id, Vector2 pos, short state, UpdatePlayerStateType st) : base(id)
        {
            MessageTime = NetTime.Now;
            Position = pos;
            StateType = st;
            PlayerState = state;
        }

        public override GameMessageType GameMessageType { get { return GameMessageType.UpdatePlayerState; } }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            MessageTime = im.ReadDouble();
            Position = im.ReadVector2();
            StateType = (UpdatePlayerStateType)im.ReadInt16();
            PlayerState = im.ReadInt16();
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write(MessageTime);
            om.Write(Position);
            om.Write((short)StateType);
            om.Write(PlayerState);
        }
    }
}
