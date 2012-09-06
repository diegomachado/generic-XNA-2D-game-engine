using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerMovementStateChangedEventArgs : PlayerStateChangedEventArgs
    {
        public Vector2 Speed { get; set; }

        public PlayerMovementStateChangedEventArgs(short playerId, Vector2 position, Vector2 speed, short state, UpdatePlayerStateType movementType) : base(playerId, position, state, movementType)
        {
            Speed = speed;
        }
    }
}