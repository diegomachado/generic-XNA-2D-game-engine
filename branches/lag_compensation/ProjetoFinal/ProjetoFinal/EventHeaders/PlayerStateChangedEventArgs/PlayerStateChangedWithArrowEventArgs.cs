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

        public PlayerStateChangedWithArrowEventArgs(short playerId, Vector2 position, Vector2 shotSpeed, short state, UpdatePlayerStateType movementType)
            : base(playerId, position, state, movementType)
        {
            ShotSpeed = shotSpeed;
        }
    }
}