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
    class VerticalIdleState : VerticalMovementState
    {
        public override VerticalMovementState Update(short playerId, GameTime gameTime, Player player, Layer collisionLayer, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            Rectangle collisionBoxVerticalOffset = player.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (!checkVerticalCollision(collisionBoxVerticalOffset, player.Speed, collisionLayer))
                return playerStates[VerticalStateType.Jumping];
            else
                return this;
            
            /*player.SpeedY = MathHelper.Clamp(player.Speed.Y, player.JumpForce.Y, 10);

            bool collidedVertically = handleVerticalCollision(player, collisionLayer);

            if (collidedVertically && player.Speed.Y > 0)
                return playerStates[VerticalStateType.Idle];
            else
                return this;*/
        }

        public override VerticalMovementState Jumped(short playerId, Player player, Dictionary<VerticalStateType, VerticalMovementState> playerStates)
        {
            player.Speed += player.JumpForce;
            return playerStates[VerticalStateType.Jumping];
        }

        public override string ToString()
        {
            return "Vertical Idle";
        }
    }
}
