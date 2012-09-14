using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateUpdatedEventArgs : PlayerStateChangedEventArgs
    {
        public double MessageTime { get; set; }
        public double LocalTime   { get; set; }
        
        public PlayerStateUpdatedEventArgs(short playerId, Vector2 position, short playerState, UpdatePlayerStateType stateType, double messageTime, double localTime) : base(playerId, position, playerState, stateType)
        {
            MessageTime = messageTime;
            LocalTime = localTime;
        }
    }
}