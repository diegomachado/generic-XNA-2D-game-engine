using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using Lidgren.Network.Xna;
using ProjetoFinal.Entities;

namespace ProjetoFinal.Network.Messages.UpdatePlayerStateMessages
{
    class UpdatePlayerStateWithArrowMessage : UpdatePlayerStateMessage
    {
        public Vector2 ShotSpeed { get; set; }

        public UpdatePlayerStateWithArrowMessage(short id, Vector2 position, Vector2 shotSpeed, short state, UpdatePlayerStateType mt) : base(id, position, state, mt)
        {
            this.ShotSpeed = shotSpeed;
        }

        public override void Decode(NetIncomingMessage im)
        {
            base.Decode(im);

            ShotSpeed = im.ReadVector2();
        }

        public override void Encode(NetOutgoingMessage om)
        {
            base.Encode(om);

            om.Write(ShotSpeed);
        }
    }
}
