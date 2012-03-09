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
        public short id { get; set; }
        public double messageTime { get; set; }
        public Vector2 position { get; set; }

        public UpdatePlayerStateMessage(NetIncomingMessage im)
        {
            Decode(im);
        }

        public UpdatePlayerStateMessage(Player player)
        {
            id = player.id;
            position = player.position;
            messageTime = NetTime.Now;
        }

        public GameMessageTypes MessageType
        {
            get { return GameMessageTypes.UpdatePlayerState; }
        }

        public void Decode(NetIncomingMessage im)
        {
            id = im.ReadInt16();
            messageTime = im.ReadDouble();
            position = im.ReadVector2();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(id);
            om.Write(messageTime);
            om.Write(position);
        }
    }
}