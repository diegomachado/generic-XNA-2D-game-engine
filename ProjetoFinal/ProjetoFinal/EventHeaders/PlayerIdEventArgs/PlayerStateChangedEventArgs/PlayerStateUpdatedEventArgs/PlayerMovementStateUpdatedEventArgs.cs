using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerMovementStateUpdatedEventArgs : PlayerStateUpdatedEventArgs
    {
        public Vector2 Speed { get; set; }

        public PlayerMovementStateUpdatedEventArgs(short playerId, Vector2 position, Vector2 speed, short playerState, UpdatePlayerStateType stateType, double messageTime, double localTime)
            : base(playerId, position, playerState, stateType, messageTime, localTime)
        {
            Speed = speed;
        }
    }
}