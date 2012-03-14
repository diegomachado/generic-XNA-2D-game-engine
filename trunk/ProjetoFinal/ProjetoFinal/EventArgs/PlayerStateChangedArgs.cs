using ProjetoFinal.Entities;

namespace ProjetoFinal.EventArgs
{
    using System;

    class PlayerStateChangedArgs : EventArgs
    {
        public short id { get; set; }
        public Player player { get; set; }

        public PlayerStateChangedArgs(short id, Player player)
        {
            this.id = id;
            this.player = player;
        }
    }
}
