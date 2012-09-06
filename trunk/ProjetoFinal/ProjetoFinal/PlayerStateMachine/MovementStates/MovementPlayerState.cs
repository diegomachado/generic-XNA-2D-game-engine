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
    abstract class MovementPlayerState : PlayerState
    {
        protected override void OnPlayerStateChanged(short playerId, Player player, UpdatePlayerStateType movementType, short nextState)
        {
            switch (movementType)
            {
                case UpdatePlayerStateType.Horizontal:
                    player.HorizontalState = (HorizontalStateType)nextState;
                    break;
                case UpdatePlayerStateType.Vertical:
                    player.VerticalState = (VerticalStateType)nextState;
                    break;
            }

            EventManager.Instance.ThrowPlayerMovementStateChanged(this, new PlayerMovementStateChangedEventArgs(playerId, player.position, player.speed, nextState, movementType));
        }
    }
}
