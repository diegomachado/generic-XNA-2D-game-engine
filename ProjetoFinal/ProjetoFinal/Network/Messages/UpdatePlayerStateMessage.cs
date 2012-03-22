using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    class UpdatePlayerStateMessage : IGameMessage
    {
        public short playerId { get; set; }
        public double messageTime { get; set; }
        public Vector2 position { get; set; }
        public PlayerState playerState { get; set; }
        public Vector2 speed { get; set; }

        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public UpdatePlayerStateMessage(short id, Player player)
        {
            playerId = id;
            messageTime = NetTime.Now;
            position = player.position;
            playerState = player.state;
            speed = player.speed;
        }

        public GameMessageType MessageType
        {
            get { return GameMessageType.UpdatePlayerState; }
        }

        public void Decode(NetIncomingMessage im)
        {
            playerId = im.ReadInt16();
            messageTime = im.ReadDouble();
            position = im.ReadVector2();
            playerState = (PlayerState)im.ReadInt16();
            speed = im.ReadVector2();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(playerId);
            om.Write(messageTime);
            om.Write(position);
            om.Write((short)playerState);
            om.Write(speed);
        }
    }
}