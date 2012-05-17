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
    abstract class JumpingSideways : JumpingState
    {
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer)
        {
            base.Update(gameTime, localPlayer, collisionLayer);

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            localPlayer.SpeedX *= localPlayer.Friction;

            // So player doesn't slide forever
            if (Math.Abs(localPlayer.Speed.X) < 0.2)
            {
                localPlayer.SpeedX = 0;

                return new JumpingStraightState();
            }

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.SpeedY = 0;

                return getWalkingState();
            }
            
            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            bool collidedHorizontally = handleHorizontalCollision(localPlayer, collisionLayer);
            bool collidedVertically = handleVerticalCollision(localPlayer, collisionLayer);

            if (collidedHorizontally && collidedVertically && localPlayer.Speed.Y > 0)
                return new IdleState();
            else if (collidedHorizontally)
                return new JumpingStraightState();
            else if (collidedVertically && localPlayer.Speed.Y > 0)
                return getWalkingState();
            else
                return this;
        }

        abstract protected LocalPlayerState getWalkingState();
    }
}
