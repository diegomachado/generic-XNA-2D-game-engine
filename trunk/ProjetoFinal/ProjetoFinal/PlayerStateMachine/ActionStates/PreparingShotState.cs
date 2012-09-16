using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;
using ProjetoFinal.PlayerStateMachine;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class PreparingShotState : ActionState
    {
        public override ActionState Update(short playerId, GameTime gameTime, Player player, Dictionary<ActionStateType, ActionState> playerStates)
        {
            // TODO: Animação de preparando tiro entra aqui
            return this;
        }

        Vector2 arrowSpeed;
        public override ActionState ShotReleased(short playerId, Player player, float arrowPower, Vector2 aim, Dictionary<ActionStateType, ActionState> playerStates)
        {
            arrowPower /= 50;
            arrowSpeed = new Vector2(aim.X - player.Center.X, aim.Y - player.Center.Y);
            arrowSpeed.Normalize();
            arrowSpeed *= arrowPower;
            player.FacingRight = (arrowSpeed.X > 0);

            OnPlayerStateChangedWithArrow(playerId, player, arrowSpeed, UpdatePlayerStateType.Action, (short)ActionStateType.Shooting);
            return playerStates[ActionStateType.Shooting];
        }

        public override string ToString()
        {
            return "Action Shooting";
        }
    }
}
