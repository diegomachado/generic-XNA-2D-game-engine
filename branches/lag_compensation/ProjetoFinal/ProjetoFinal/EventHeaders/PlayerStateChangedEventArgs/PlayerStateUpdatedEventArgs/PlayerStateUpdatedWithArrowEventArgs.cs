using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateUpdatedWithArrowEventArgs : PlayerStateUpdatedEventArgs
    {
        public Vector2 ShotSpeed { get; set; }

        public PlayerStateUpdatedWithArrowEventArgs(short playerId, Vector2 position, Vector2 shotSpeed, short playerState, UpdatePlayerStateType stateType, double messageTime, double localTime)
            : base(playerId, position, playerState, stateType, messageTime, localTime)
        {
            ShotSpeed = shotSpeed;
        }
    }
}