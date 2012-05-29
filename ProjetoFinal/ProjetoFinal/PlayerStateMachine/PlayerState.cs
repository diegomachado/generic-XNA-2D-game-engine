using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers;
using ProjetoFinal.Managers.LocalPlayerStates;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.PlayerStateMachine
{
    abstract class PlayerState
    {
        protected void OnPlayerStateChanged(short playerId, Player player, UpdatePlayerStateMessageType messageType, short nextState)
        {
            switch (messageType)
            {
                case UpdatePlayerStateMessageType.Horizontal:
                    player.LastHorizontalState = (HorizontalStateType)nextState;
                    break;
                case UpdatePlayerStateMessageType.Vertical:
                    player.LastVerticalState = (VerticalStateType)nextState;
                    break;
            }

            EventManager.Instance.throwPlayerStateChanged(playerId, player, messageType);
        }
    }
}
