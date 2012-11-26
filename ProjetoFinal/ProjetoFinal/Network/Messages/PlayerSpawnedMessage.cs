using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerSpawnedMessage : PlayerCreatedMessage
    {
        public override GameMessageType GameMessageType  { get { return GameMessageType.PlayerSpawned; } }

        public PlayerSpawnedMessage(short playerId, Vector2 spawnPosition) : base(playerId, spawnPosition)
        {

        }

        public PlayerSpawnedMessage(NetIncomingMessage im) : base(im)
        {

        }
    }
}
