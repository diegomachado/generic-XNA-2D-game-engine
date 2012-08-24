using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateUpdatedEventArgs : EventArgs
    {
        public short PlayerId     { get; set; }
        public double MessageTime { get; set; }
        public Vector2 Position   { get; set; }
        public short PlayerState  { get; set; }
        public Vector2 Speed      { get; set; }
        public double LocalTime   { get; set; }
        public UpdatePlayerStateMessageType MovementType { get; set; }

        public PlayerStateUpdatedEventArgs(UpdatePlayerMovementStateMessage updatePlayerStateMessage, double localTime)
        {
            this.PlayerId = updatePlayerStateMessage.playerId;
            this.MessageTime = updatePlayerStateMessage.messageTime;
            this.Position = updatePlayerStateMessage.position;
            this.PlayerState = updatePlayerStateMessage.playerState;
            this.Speed = updatePlayerStateMessage.speed;
            this.MovementType = updatePlayerStateMessage.messageType;
            this.LocalTime = localTime;
        }
    }
}