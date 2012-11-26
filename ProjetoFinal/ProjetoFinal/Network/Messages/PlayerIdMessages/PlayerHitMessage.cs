using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerHitMessage : PlayerIdMessage
    {
        public short AttackerId { get; set; }

        public override GameMessageType GameMessageType { get { return GameMessageType.PlayerHit; } }

        public PlayerHitMessage(NetIncomingMessage im) : base(im)
        {
        
        }

        public PlayerHitMessage(short playerId, short attackerId) : base(playerId)
        {
            this.AttackerId = attackerId;
        }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            AttackerId = im.ReadInt16();
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write(AttackerId);
        }
    }
}
