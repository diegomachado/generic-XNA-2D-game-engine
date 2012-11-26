using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class NewClientPlayerCreatedEventArgs : EventArgs
    {
        public short PlayerId { get; set; }
        public Dictionary<short, Vector2> PlayerPositions { get; set; }

        public NewClientPlayerCreatedEventArgs(short playerId, Dictionary<short, Vector2> playerPositions)
        {
            PlayerId = playerId;
            PlayerPositions = playerPositions;
        }
    }
}