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
    abstract class MovingOnAirSideways : JumpingState
    {
        public MovingOnAirSideways(bool isLocal) : base(isLocal) { }

        public override PlayerState Update(short playerId, GameTime gameTime, Player localPlayer, Layer collisionLayer, Dictionary<PlayerStateType, PlayerState> localPlayerStates)
        {
            base.Update(playerId, gameTime, localPlayer, collisionLayer, localPlayerStates);

            Rectangle collisionBoxVerticalOffset = localPlayer.CollisionBox;
            collisionBoxVerticalOffset.Offset(0, 1);

            localPlayer.SpeedX *= localPlayer.Friction;

            if (clampHorizontalSpeed(localPlayer))
            {
                //OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingStraight);
                return localPlayerStates[PlayerStateType.JumpingStraight];
            }

            if (checkVerticalCollision(collisionBoxVerticalOffset, localPlayer.Speed, collisionLayer))
            {
                localPlayer.OnGround = true;
                localPlayer.SpeedY = 0;

                //OnPlayerStateChanged(playerId, localPlayer, getWalkingState());
                return localPlayerStates[getWalkingState()];
            }
            
            localPlayer.SpeedY = MathHelper.Clamp(localPlayer.Speed.Y, localPlayer.JumpForce.Y, 10);

            bool collidedHorizontally = handleHorizontalCollision(localPlayer, collisionLayer);
            bool collidedVertically = handleVerticalCollision(localPlayer, collisionLayer);

            if (collidedHorizontally && collidedVertically && localPlayer.Speed.Y > 0)
            {
                //OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.Idle);
                return localPlayerStates[PlayerStateType.Idle];
            }
            else if (collidedHorizontally)
            {
                //OnPlayerStateChanged(playerId, localPlayer, PlayerStateType.JumpingStraight);
                return localPlayerStates[PlayerStateType.JumpingStraight];
            }
            else if (collidedVertically && localPlayer.Speed.Y > 0)
            {
                //OnPlayerStateChanged(playerId, localPlayer, getWalkingState());
                return localPlayerStates[getWalkingState()];
            }
            else
            {
                return this;
            }
        }

        abstract protected PlayerStateType getWalkingState();
    }
}
