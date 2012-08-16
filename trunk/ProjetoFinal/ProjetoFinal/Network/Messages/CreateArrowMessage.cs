using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

namespace ProjetoFinal.Network.Messages
{
    class CreateArrowMessage : IGameMessage
    {
        public short playerId     { get; set; }
        public double messageTime { get; set; }
        public short playerState  { get; set; }
        public Vector2 position   { get; set; }
        public Vector2 speed      { get; set; }

        public CreateArrowMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public CreateArrowMessage(short id, Player player)
        {
            playerId = id;
            messageTime = NetTime.Now;
            position = player.Position;// TODO: mudar
            playerState = (short)player.LastActionState;
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
            playerState = im.ReadInt16();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(playerId);
            om.Write(messageTime);
            om.Write(position);
            om.Write(playerState);
        }
    }
}