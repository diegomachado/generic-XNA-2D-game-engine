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
    class JumpingStraightState : JumpingState
    {
        public override LocalPlayerState Update(GameTime gameTime, Player localPlayer, Layer collisionLayer)
        {
            base.Update(gameTime, localPlayer, collisionLayer);

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.Speed = Vector2.Zero;

                return new IdleState();
            }
            
            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            if (handleVerticalCollision(localPlayer, collisionLayer) && localPlayer.Speed.Y > 0)
                return new IdleState();                    
            else
                return this;
        }

        public override LocalPlayerState MovingLeft(Player localPlayer)
        {
            localPlayer.Speed -= localPlayer.walkForce;
            localPlayer.FacingLeft = true;

            return new JumpingLeftState();
        }

        public override LocalPlayerState MovingRight(Player localPlayer)
        {
            localPlayer.Speed += localPlayer.walkForce;
            localPlayer.FacingLeft = false;

            return new JumpingRightState();
        }

        public override string ToString()
        {
            return "JumpingStraight";
        }
    }
}
