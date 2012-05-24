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
    class StoppingWalkingRightState : SidewaysState
    {
        public override SidewaysState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<HorizontalStateType, SidewaysState> playerStates)
        {
            player.SpeedX *= player.Friction;

            if (clampHorizontalSpeed(player) || handleHorizontalCollision(player, collisionLayer))
                return playerStates[HorizontalStateType.Idle];
            else
                return this;
        }

        public override SidewaysState MovedLeft(short playerId, Player player, Dictionary<HorizontalStateType, SidewaysState> playerStates)
        {
            return playerStates[HorizontalStateType.WalkingLeft];
        }

        public override SidewaysState MovedRight(short playerId, Player player, Dictionary<HorizontalStateType, SidewaysState> playerStates)
        {
            return playerStates[HorizontalStateType.WalkingRight];
        }

        public override string ToString()
        {
            return "StoppingWalkingRight";
        }
    }
}
