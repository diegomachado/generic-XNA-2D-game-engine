using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

namespace ProjetoFinal.Network.Messages
{
    class UpdatePlayerStateMessage : IGameMessage
    {
        public short playerId { get; set; }
        public double messageTime { get; set; }
        public Vector2 position { get; set; }
        public PlayerStateType playerState { get; set; }
        public Vector2 speed { get; set; }
        public PlayerStateMessage playerStateMessage { get; set; }

        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public UpdatePlayerStateMessage(short id, Player player, PlayerStateMessage message)
        {
            playerId = id;
            messageTime = NetTime.Now;
            position = player.Position;
            playerState = player.State;
            playerStateMessage = message;
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
            playerState = (PlayerStateType)im.ReadInt16();
            playerStateMessage = (PlayerStateMessage)im.ReadInt16();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(playerId);
            om.Write(messageTime);
            om.Write(position);
            om.Write((short)playerState);
            om.Write((short)playerStateMessage);
        }
    }
}