using System.Collections.Generic;
using ProjetoFinal.Managers.LocalPlayerStates;
using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;

using OgmoEditorLibrary;

namespace ProjetoFinal.PlayerStateMachine.VerticalMovementStates
{
    class StartedJumpingState : JumpingState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            player.Jump();
            VerticalMovementState state = base.Update(playerId, gameTime, player, grid, playerStates);

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
