using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.EventHeaders;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.PlayerStateMachine
{
    abstract class PlayerState
    {
        abstract protected void OnPlayerStateChanged(short playerId, Player player, UpdatePlayerStateType movementType, short nextState);
    }
}
