using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateUpdatedEventArgs : EventArgs
    {
        public short playerId     { get; set; }
        public double messageTime { get; set; }
        public Vector2 position   { get; set; }
        public short playerState  { get; set; }
        public Vector2 speed      { get; set; }
        public double localTime   { get; set; }
        public UpdatePlayerStateMessageType movementType { get; set; }

        public PlayerStateUpdatedEventArgs()
        {
        }

        public PlayerStateUpdatedEventArgs(UpdatePlayerStateMessage updatePlayerStateMessage, double localTime)
        {
            this.playerId = updatePlayerStateMessage.playerId;
            this.messageTime = updatePlayerStateMessage.messageTime;
            this.position = updatePlayerStateMessage.position;
            this.playerState = updatePlayerStateMessage.playerState;
            this.speed = updatePlayerStateMessage.speed;
            this.movementType = updatePlayerStateMessage.movementType;
            this.localTime = localTime;
        }
    }
}