using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerHitEventArgs : EventArgs
    {
        public short PlayerId { get; set; }
        public short AttackerId { get; set; }

        public PlayerHitEventArgs(short playerId, short attackerId)
        {
            PlayerId = playerId;
            AttackerId = attackerId;
        }
    }
}