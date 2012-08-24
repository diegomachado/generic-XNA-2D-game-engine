using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers.LocalPlayerStates;

namespace ProjetoFinal.Network.Messages
{
    class UpdatePlayerMovementStateMessage : UpdatePlayerStateMessage
    {
        public Vector2 speed { get; set; }
        public short playerState { get; set; }

        public UpdatePlayerMovementStateMessage(NetIncomingMessage im)
            : base(im)
        {

        }

        public UpdatePlayerMovementStateMessage(short id, Player player, UpdatePlayerStateMessageType mt) : base(id, player, mt)
        {
            speed = player.Speed;

            switch (mt)
            {
                case UpdatePlayerStateMessageType.Horizontal:
                    playerState = (short)player.HorizontalState;
                    break;
                case UpdatePlayerStateMessageType.Vertical:
                    playerState = (short)player.VerticalState;
                    break;
            }

            messageType = mt;
        }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            speed = im.ReadVector2();
            playerState = im.ReadInt16();
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write(speed);
            om.Write(playerState);
        }
    }
}