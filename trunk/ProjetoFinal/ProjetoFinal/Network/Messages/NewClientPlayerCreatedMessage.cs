using System;
using System.Collections.Generic;

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class NewClientPlayerCreatedMessage : IGameMessage
    {
        public Dictionary<short, Vector2> PlayerPositions { get; set; }

        public GameMessageType GameMessageType { get { return GameMessageType.NewClientPlayerCreated; } }

        public NewClientPlayerCreatedMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public NewClientPlayerCreatedMessage(Dictionary<short, Vector2> playerPositions)
        {
            PlayerPositions = playerPositions;
        }

        public void Decode(NetIncomingMessage im)
        {
            int numPlayers = im.ReadInt32();

            if (PlayerPositions == null)
                PlayerPositions = new Dictionary<short, Vector2>();

            for (int i = 0; i < numPlayers; i++)
                PlayerPositions.Add(im.ReadInt16(), im.ReadVector2());
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(PlayerPositions.Count);

            foreach (KeyValuePair<short, Vector2> playerPosition in PlayerPositions)
            {
                om.Write(playerPosition.Key);
                om.Write(playerPosition.Value);
            }
        }
    }
}
