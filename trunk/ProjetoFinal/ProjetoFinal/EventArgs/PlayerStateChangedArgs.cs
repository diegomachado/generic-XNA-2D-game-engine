using ProjetoFinal.Entities;

namespace ProjetoFinal.EventArgs
{
    using System;

    class PlayerStateChangedArgs : EventArgs
    {
        public Player player { get; set; }

        public PlayerStateChangedArgs(Player player)
        {
            this.player = player;
        }
    }
}
