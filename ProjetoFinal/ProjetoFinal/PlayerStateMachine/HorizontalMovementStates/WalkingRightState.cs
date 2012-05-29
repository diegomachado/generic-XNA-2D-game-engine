using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ProjetoFinal.Entities;
using OgmoLibrary;

namespace ProjetoFinal.Managers.LocalPlayerStates
{
    class WalkingRightState : HorizontalMovementState
    {
        public override HorizontalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            player.FacingLeft = false;            
            player.Speed += player.walkForce;
            player.SpeedX *= player.Friction;

            if (player.isMovingHorizontally || handleHorizontalCollision(player, collisionLayer, elapsedTime))
                return playerStates[HorizontalStateType.Idle];
            else
                return this;
        }

        public override HorizontalMovementState StoppedMovingRight(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return playerStates[HorizontalStateType.StoppingWalkingRight];
        }

        public override HorizontalMovementState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, HorizontalMovementState> playerStates)
        {
            return playerStates[HorizontalStateType.WalkingLeft];
        }

        public override string ToString()
        {
            return "WalkingRight";
        }
    }
}
