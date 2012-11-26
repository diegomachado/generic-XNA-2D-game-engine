using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

using ProjetoFinal.Managers;
using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages
{
    class UpdatePlayerMovementStateMessage : UpdatePlayerStateMessage
    {
        public Vector2 Speed { get; set; }

        public UpdatePlayerMovementStateMessage(NetIncomingMessage im)
            : base(im)
        {

        }

        public UpdatePlayerMovementStateMessage(short id, Vector2 position, Vector2 speed, short state, UpdatePlayerStateType mt)
            : base(id, position, state, mt)
        {
            Speed = speed;
        }

        public override GameMessageType GameMessageType { get { return GameMessageType.UpdatePlayerMovementState; } }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            Speed = im.ReadVector2();
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write(Speed);
        }
    }
}