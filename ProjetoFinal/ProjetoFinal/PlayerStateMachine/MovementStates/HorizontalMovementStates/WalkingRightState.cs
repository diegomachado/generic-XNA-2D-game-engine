using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;
using ProjetoFinal.Network.Messages;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class WalkingRightState : HorizontalMovementState
    {
        public override HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            player.FacingLeft = false;
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
