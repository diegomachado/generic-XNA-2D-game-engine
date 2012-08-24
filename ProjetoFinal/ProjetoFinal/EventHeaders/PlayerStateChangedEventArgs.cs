using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerStateChangedEventArgs : EventArgs
    {
        public short PlayerId     { get; set; }
        public Player Player      { get; set; }
        public UpdatePlayerStateMessageType MovementType { get; set; }

        public PlayerStateChangedEventArgs(short playerId, Player player, UpdatePlayerStateMessageType movementType)
        {
            this.PlayerId = playerId;
            this.Player = player;
            this.MovementType = movementType;
        }
    }
}