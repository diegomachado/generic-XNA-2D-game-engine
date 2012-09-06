using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoFinal.Managers.LocalPlayerStates;
using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;
using OgmoLibrary;

namespace ProjetoFinal.PlayerStateMachine.VerticalMovementStates
{
    class StartedJumpingState : JumpingState
    {
        VerticalMovementState state;

        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            player.Jump();
            player.MoveYBy(player.speed.Y);

            state = base.Update(playerId, gameTime, player, collisionLayer, playerStates);

            if (state == this)
                return playerStates[VerticalStateType.Jumping];
            else
                return state;
        }

        public override string ToString()
        {
            return "Started Jumping";
        }
    }
}
