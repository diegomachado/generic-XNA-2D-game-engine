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
    class JumpingLeftState : LocalPlayerState
    {
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer)
        {
            localPlayer.Speed += localPlayer.Gravity;

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            localPlayer.SpeedX *= localPlayer.Friction;

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.SpeedY = 0;

                return new WalkingLeftState();
            }

            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            bool collidedHorizontally = handleHorizontalCollision(localPlayer, collisionLayer);
            bool collidedVertically = handleVerticalCollision(localPlayer, collisionLayer);

            if (collidedHorizontally && collidedVertically && localPlayer.Speed.Y > 0)
                return new IdleState();
            else if (collidedHorizontally)
                return new JumpingState();
            else if (collidedVertically && localPlayer.Speed.Y > 0)
                return new WalkingLeftState();
            else
                return this;
        }

        public override LocalPlayerState MovingLeft(Player localPlayer)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return this;
        }

        public override LocalPlayerState MovingRight(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return new JumpingRightState();
        }

        public override string ToString()
        {
            return "JumpingLeft";
        }
    }
}
