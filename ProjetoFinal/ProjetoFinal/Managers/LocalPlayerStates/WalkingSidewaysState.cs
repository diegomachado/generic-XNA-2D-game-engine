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
    abstract class WalkingSidewaysState : LocalPlayerState
    {
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer)
        {
            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (!checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = false;

                return getWalkingState();
                //return new JumpingRightState();
            }

            localPlayer.SpeedX *= localPlayer.Friction;

            // So player doesn't slide forever
            if (Math.Abs(localPlayer.Speed.X) < 0.2)
            {
                localPlayer.SpeedX = 0;

                return new IdleState();
            }

            if (handleHorizontalCollision(localPlayer, collisionLayer))
                return new IdleState();
            else
                return this;
        }

        abstract protected LocalPlayerState getWalkingState();
    }
}
