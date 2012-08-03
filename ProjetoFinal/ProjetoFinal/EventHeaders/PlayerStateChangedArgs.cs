using ProjetoFinal.Entities;

namespace ProjetoFinal.EventArgs
{
    using System;
    using ProjetoFinal.Managers.LocalPlayerStates;
    using ProjetoFinal.Network.Messages;

    class PlayerStateChangedArgs : EventArgs
    {
        public short id { get; set; }
        public Player player { get; set; }
        public UpdatePlayerStateMessageType messageType { get; set; }

        public PlayerStateChangedArgs(short id, Player player, UpdatePlayerStateMessageType messageType)
        {
            this.id = id;
            this.player = player;
            this.messageType = messageType;
        }
    }
}