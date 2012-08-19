using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
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

        public override ActionState ShotReleased(short playerId, Player player, float shootingTimer, Point aim, Dictionary<ActionStateType, ActionState> playerStates)
        {
            Vector2 speed = new Vector2(aim.X - player.Position.X, aim.Y - player.Position.Y);
            speed.Normalize();
            speed *= (shootingTimer / 100);

            // TODO: Mudar posição de saída da flecha posteriormente
            OnArrowShot(playerId, player.WeaponPosition, speed);

            return playerStates[ActionStateType.Shooting];
        }

        public override string ToString()
        {
            return "Action Shooting";
        }
    }
}
