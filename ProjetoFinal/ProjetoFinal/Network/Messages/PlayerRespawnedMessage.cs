using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerRespawnedMessage : IGameMessage
    {
        public short PlayerId { get; set; }
        public Vector2 RespawnPosition { get; set; }

        public GameMessageType GameMessageType { get { return GameMessageType.PlayerRespawned; } }

        public PlayerRespawnedMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public PlayerRespawnedMessage(short playerId, Vector2 respawnPosition)
        {
            this.PlayerId = playerId;
            this.RespawnPosition = respawnPosition;
        }

        public void Decode(NetIncomingMessage im)
        {
            PlayerId = im.ReadInt16();
            RespawnPosition = im.ReadVector2();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(PlayerId);
            om.Write(RespawnPosition);
        }
    }
}
