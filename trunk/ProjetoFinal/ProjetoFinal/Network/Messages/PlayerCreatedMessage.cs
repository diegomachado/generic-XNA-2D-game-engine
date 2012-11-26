using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerCreatedMessage : IGameMessage
    {
        public short PlayerId { get; set; }
        public Vector2 SpawnPosition { get; set; }

        public virtual GameMessageType GameMessageType { get { return GameMessageType.PlayerCreated; } }

        public PlayerCreatedMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public PlayerCreatedMessage(short playerId, Vector2 spawnPosition)
        {
            this.PlayerId = playerId;
            this.SpawnPosition = spawnPosition;
        }

        public void Decode(NetIncomingMessage im)
        {
            PlayerId = im.ReadInt16();
            SpawnPosition = im.ReadVector2();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(PlayerId);
            om.Write(SpawnPosition);
        }
    }
}
