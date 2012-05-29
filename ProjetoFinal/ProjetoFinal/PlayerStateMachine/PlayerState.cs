using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers;

namespace ProjetoFinal.PlayerStateMachine
{
    abstract class PlayerState
    {
        protected void OnPlayerStateChanged(short playerId, Player player)
        {
            EventManager.Instance.throwPlayerStateChanged(playerId, player);
        }
    }
}
