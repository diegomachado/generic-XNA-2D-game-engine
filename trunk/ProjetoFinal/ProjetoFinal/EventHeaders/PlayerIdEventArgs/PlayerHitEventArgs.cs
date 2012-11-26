using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerHitEventArgs : PlayerIdEventArgs
    {
        public short AttackerId { get; set; }

        public PlayerHitEventArgs(short playerId, short attackerId) : base(playerId)
        {
            AttackerId = attackerId;
        }

        public PlayerHitEventArgs(PlayerHitMessage playerHitMessage)
        {
            PlayerId = playerHitMessage.PlayerId;
            AttackerId = playerHitMessage.AttackerId;
        }
    }
}