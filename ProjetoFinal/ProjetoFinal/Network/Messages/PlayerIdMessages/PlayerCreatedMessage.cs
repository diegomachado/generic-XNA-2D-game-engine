using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class PlayerCreatedMessage : PlayerIdMessage
    {
        public Vector2 SpawnPosition { get; set; }

        public override GameMessageType GameMessageType { get { return GameMessageType.PlayerCreated; } }

        public PlayerCreatedMessage(NetIncomingMessage im) : base(im)
        {

        }

        public PlayerCreatedMessage(short playerId, Vector2 spawnPosition) : base(playerId)
        {
            this.SpawnPosition = spawnPosition;
        }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            SpawnPosition = im.ReadVector2();
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write(SpawnPosition);
        }
    }
}
