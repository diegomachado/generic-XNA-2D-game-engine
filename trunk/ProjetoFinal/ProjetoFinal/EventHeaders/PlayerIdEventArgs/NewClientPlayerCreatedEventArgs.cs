using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class NewClientPlayerCreatedEventArgs : PlayerIdEventArgs
    {
        public Dictionary<short, Vector2> PlayerPositions { get; set; }

        public NewClientPlayerCreatedEventArgs(short playerId, Dictionary<short, Vector2> playerPositions) : base(playerId)
        {
            PlayerPositions = playerPositions;
        }
    }
}