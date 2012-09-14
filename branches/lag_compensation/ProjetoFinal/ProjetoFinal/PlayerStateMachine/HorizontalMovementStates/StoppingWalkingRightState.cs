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
    class StoppingWalkingRightState : HorizontalMovementState
    {
        public override HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            player.speed.X *= player.friction;
            player.MoveXBy(player.speed.X);

            if (player.IsMovingHorizontally() || player.MapCollideX(player.speed.X))
                return playerStates[HorizontalStateType.Idle];
            else
                return this;
        }

        public override HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateMessageType.Horizontal, (short)HorizontalStateType.WalkingLeft);
            return playerStates[HorizontalStateType.WalkingLeft];
        }
        public override HorizontalMovementState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            OnPlayerStateChanged(playerId, player, UpdatePlayerStateMessageType.Horizontal, (short)HorizontalStateType.WalkingRight);
            return playerStates[HorizontalStateType.WalkingRight];
        }
        public override string ToString()
        {
            return "StoppingWalkingRight";
        }
    }
}
