using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerSpawnedEventArgs : PlayerIdEventArgs
    {
        public Vector2 SpawnPoint { get; set; }

        public PlayerSpawnedEventArgs(short playerId, Vector2 spawnPoint) : base(playerId)
        {
            SpawnPoint = spawnPoint;
        }
    }
}