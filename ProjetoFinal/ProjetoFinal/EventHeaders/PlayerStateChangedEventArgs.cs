using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateChangedEventArgs : EventArgs
    {
        public short playerId     { get; set; }
        public Player player      { get; set; }
        public UpdatePlayerStateMessageType movementType { get; set; }

        public PlayerStateChangedEventArgs(short playerId, Player player, UpdatePlayerStateMessageType movementType)
        {
            this.playerId = playerId;
            this.player = player;
            this.movementType = movementType;
        }
    }
}