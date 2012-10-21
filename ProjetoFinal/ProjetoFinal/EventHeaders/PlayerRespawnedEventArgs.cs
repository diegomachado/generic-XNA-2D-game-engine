using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerRespawnedEventArgs : EventArgs
    {
        public short PlayerId { get; set; }
        public Vector2 RespawnPoint { get; set; }

        public PlayerRespawnedEventArgs(short playerId, Vector2 respawnPoint)
        {
            PlayerId = playerId;
            RespawnPoint = respawnPoint;
        }

        //public PlayerRespawnedEventArgsPlayerHitEventArgs(PlayerHitMessage playerHitMessage)
        //{
        //    PlayerId = playerHitMessage.PlayerId;
        //    AttackerId = playerHitMessage.AttackerId;
        //}
    }
}