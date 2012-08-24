using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateChangedWithArrowEventArgs : PlayerStateChangedEventArgs
    {
        public Vector2 ShotSpeed { get; set; }

        public PlayerStateChangedWithArrowEventArgs(short playerId, Player player, Vector2 speed, UpdatePlayerStateMessageType movementType) : base(playerId, player, movementType)
        {
            ShotSpeed = speed;
        }
    }
}