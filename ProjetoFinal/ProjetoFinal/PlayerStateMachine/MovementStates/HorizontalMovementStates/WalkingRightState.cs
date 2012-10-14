using System.Collections.Generic;
using Microsoft.Xna.Framework;

using ProjetoFinal.Entities;
using ProjetoFinal.Network.Messages;

using OgmoEditorLibrary;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class WalkingRightState : HorizontalMovementState
    {
        public override HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Grid grid, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            player.FacingRight = true;
            player.speed.X += player.moveSpeed;
            player.speed.X *= player.friction;

            player.MoveXBy(player.speed.X);

            if (!player.IsMovingHorizontally())       
                return playerStates[HorizontalStateType.Idle];
            else
                return this;
        }

        public override HorizontalMovementState StoppedMovingRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Horizontal, (short)HorizontalStateType.StoppingWalkingRight);

            return playerStates[HorizontalStateType.StoppingWalkingRight];
        }

        public override HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateType.Horizontal, (short)HorizontalStateType.WalkingLeft);

            return playerStates[HorizontalStateType.WalkingLeft];
        }

        public override string ToString()
        {
            return "WalkingRight";
        }
    }
}
