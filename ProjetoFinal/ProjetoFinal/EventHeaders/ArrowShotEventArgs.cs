using System;
using System.Collections.Generic;

using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.EventHeaders
{
    class ArrowShotEventArgs : EventArgs
    {
        public short playerId { get; set; }
        public Vector2 speed { get; set; }
        public Vector2 position { get; set; }

        public ArrowShotEventArgs(short playerId, Vector2 position, Vector2 speed)
        {
            this.playerId = playerId;
            this.position = position;
            this.speed = speed;
        }
    }
}