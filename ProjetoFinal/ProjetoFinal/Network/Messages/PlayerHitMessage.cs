using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerHitMessage : IGameMessage
    {
        public short PlayerId { get; set; }
        public short AttackerId { get; set; }

        public GameMessageType GameMessageType { get { return GameMessageType.PlayerHit; } }

        public PlayerHitMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public PlayerHitMessage(short playerId, short attackerId)
        {
            this.PlayerId = playerId;
            this.AttackerId = attackerId;
        }

        public void Decode(NetIncomingMessage im)
        {
            PlayerId = im.ReadInt16();
            AttackerId = im.ReadInt16();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(PlayerId);
            om.Write(AttackerId);
        }
    }
}
