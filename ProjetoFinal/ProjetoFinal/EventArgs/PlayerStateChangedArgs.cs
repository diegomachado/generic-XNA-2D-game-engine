using ProjetoFinal.Entities;

namespace ProjetoFinal.EventArgs
{
    using System;
    using ProjetoFinal.Managers.LocalPlayerStates;

    class PlayerStateChangedArgs : EventArgs
    {
        public short id { get; set; }
        public Player player { get; set; }
        public PlayerStateMessage message { get; set; }

        public PlayerStateChangedArgs(short id, Player player, PlayerStateMessage message)
        {
            this.id = id;
            this.player = player;
            this.message = message;
        }
    }
}