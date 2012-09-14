using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateChangedEventArgs : EventArgs
    {
        public short PlayerId { get; set; }
        public Vector2 Position { get; set; }
        public short PlayerState { get; set; }
        public UpdatePlayerStateType StateType { get; set; }

        public PlayerStateChangedEventArgs(short playerId, Vector2 position, short state, UpdatePlayerStateType stateType)
        {
            PlayerId = playerId;
            Position = position;
            StateType = stateType;
            PlayerState = state;
        }
    }
}