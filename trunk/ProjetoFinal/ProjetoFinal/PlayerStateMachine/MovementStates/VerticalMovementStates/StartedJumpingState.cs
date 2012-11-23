using System.Collections.Generic;
using ProjetoFinal.PlayerStateMachine.MovementStates.VerticalMovementStates;
using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;

using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.PlayerStateMachine.MovementStates.VerticalMovementStates
{
    class StartedJumpingState : JumpingState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            player.Jump();
            VerticalMovementState state = base.Update(playerId, gameTime, player, grid, playerStates);

            if (state == this)
            {
                player.VerticalStateType = VerticalStateType.Jumping;
                return playerStates[VerticalStateType.Jumping];
            }
            else
            { 
                return state;
            }
        }

        public override string ToString()
        {
            return "Jumping";
        }
    }
}
