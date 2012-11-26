using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerSpawnedEventArgs : EventArgs
    {
        public short PlayerId { get; set; }
        public Vector2 SpawnPoint { get; set; }

        public PlayerSpawnedEventArgs(short playerId, Vector2 spawnPoint)
        {
            PlayerId = playerId;
            SpawnPoint = spawnPoint;
        }
    }
}