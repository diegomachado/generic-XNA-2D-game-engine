using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

namespace ProjetoFinal.Network.Messages
{
    enum UpdatePlayerStateMessageType : short
    {
        Horizontal,
        Vertical
    }

    class UpdatePlayerStateMessage : IGameMessage
    {
        public short playerId { get; set; }
        public double messageTime { get; set; }
        public Vector2 position { get; set; }
        public short playerState { get; set; }
        public Vector2 speed { get; set; }
        public UpdatePlayerStateMessageType messageType { get; set; }

        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public UpdatePlayerStateMessage(short id, Player player, UpdatePlayerStateMessageType mt)
        {
            playerId = id;
            messageTime = NetTime.Now;
            position = player.Position;

            switch (mt)
            {
                case UpdatePlayerStateMessageType.Horizontal:
                    playerState = (short)player.LastHorizontalState;
                    break;
                case UpdatePlayerStateMessageType.Vertical:
                    playerState = (short)player.LastVerticalState;
                    break;
            }

            messageType = mt;
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
            messageType = (UpdatePlayerStateMessageType)im.ReadInt16();
            playerState = im.ReadInt16();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(playerId);
            om.Write(messageTime);
            om.Write(position);
            om.Write((short)messageType);
            om.Write(playerState);
        }
    }
}