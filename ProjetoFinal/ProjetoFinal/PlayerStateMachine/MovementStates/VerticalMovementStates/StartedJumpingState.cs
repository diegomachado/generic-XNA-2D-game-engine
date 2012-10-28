using System.Collections.Generic;
using ProjetoFinal.Managers.LocalPlayerStates;
using Microsoft.Xna.Framework;
using ProjetoFinal.Entities;

using OgmoEditorLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.PlayerStateMachine.VerticalMovementStates
{
    class StartedJumpingState : JumpingState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            player.Jump();
            player.spriteMap.Play("idle");
            VerticalMovementState state = base.Update(playerId, gameTime, player, grid, playerStates);

            if (state == this)
                return playerStates[VerticalStateType.Jumping];
            else
                return state;
        }

        public override string ToString()
        {
            return "Jumping";
        }
    }
}
