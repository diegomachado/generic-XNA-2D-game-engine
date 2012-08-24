using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Entities;
using ProjetoFinal.Managers;
using ProjetoFinal.Managers.LocalPlayerStates;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.EventHeaders;
using Microsoft.Xna.Framework;

namespace ProjetoFinal.PlayerStateMachine
{
    abstract class PlayerState
    {
        protected void OnPlayerStateChanged(short playerId, Player player, UpdatePlayerStateMessageType messageType, short nextState)
        {
            switch (messageType)
            {
                case UpdatePlayerStateMessageType.Action:
                    player.ActionState = (ActionStateType)nextState;
                    break;
                case UpdatePlayerStateMessageType.Horizontal:
                    player.HorizontalState = (HorizontalStateType)nextState;
                    break;
                case UpdatePlayerStateMessageType.Vertical:
                    player.VerticalState = (VerticalStateType)nextState;
                    break;
            }

            EventManager.Instance.ThrowPlayerStateChanged(this, new PlayerStateChangedEventArgs(playerId, player, messageType));
        }
    }
}
