using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.EventHeaders
{
    class PlayerIdEventArgs : EventArgs
    {
        public short PlayerId { get; set; }

        public PlayerIdEventArgs()
        {

        }

        public PlayerIdEventArgs(short playerId)
        {
            PlayerId = playerId;
        }
    }
}